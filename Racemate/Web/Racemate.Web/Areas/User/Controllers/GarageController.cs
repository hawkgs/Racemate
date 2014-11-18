namespace Racemate.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Caching;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNet.Identity;

    using Racemate.Data;
    using Racemate.Data.Models;
    using Racemate.Web.Areas.User.ViewModels.Garage;
    using Racemate.Web.Controllers.Common;
    using Racemate.Common;

    using Racemate.Web.Infrastructure.Caching.Contracts;
    using Racemate.Web.Infrastructure.Caching;

    [Authorize]
    public class GarageController : BaseController
    {
        public GarageController(IRacemateData data)
            : base(data)
        {
            this.CarMakesCacheService = new CacheService<CarMake>("carMakes");
            this.RaceTypesCacheService = new CacheService<RaceType>("raceTypes");
        }

        public ICacheService<CarMake> CarMakesCacheService { get; private set; }

        public ICacheService<RaceType> RaceTypesCacheService { get; private set; }


        public ActionResult Index()
        {
            var userId = this.User.Identity.GetUserId();

            var cars = this.data.Cars.All()
                .Where(c => c.OwnerId == userId)
                .Project().To<CarViewModel>();

            return this.View(cars);
        }

        public ActionResult AddCar()
        {
            var model = new AddCarViewModel();
            this.AttachContentToModel(model);

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCar(AddCarViewModel model)
        {
            int carModelId;
            bool isValid = true;
            CarModel carModel = null;
            var raceTypes = this.ParseRaceTypes(model.SelectedRaceTypes);

            if (int.TryParse(this.Request["CarModelId"], out carModelId))
            {
                carModel = this.data.CarModels.GetById(carModelId);
            }

            if (carModel == null)
            {
                this.ModelState.AddModelError(String.Empty, "The car make and/or model are invalid!");
                isValid = false;
            }
            else if (raceTypes.Count == 0)
            {
                this.ModelState.AddModelError(String.Empty, "The selected race type(s) are invalid!");
                isValid = false;
            }
            else if (!this.ModelState.IsValid)
            {
                isValid = false;
            }

            if (!isValid)
            {
                this.AttachContentToModel(model);
                return this.View(model);
            }

            var car = Mapper.Map<AddCarViewModel, Car>(model);
            car.Owner = this.CurrentUser;
            car.Model = carModel;
            car.RaceTypes = raceTypes;

            this.data.Cars.Add(car);
            this.data.SaveChanges();

            return this.RedirectToAction("Index");
        }

        public JsonResult MakeModels(int makeId)
        {
            var models = this.data.CarModels.All()
                .Where(m => m.CarMakeId == makeId)
                .Project().To<CarModelViewModel>();

            return this.Json(models, JsonRequestBehavior.AllowGet);
        }

        #region Helpers

        private void AttachContentToModel(AddCarViewModel model)
        {
            model.RaceTypesList = this.BuildRaceTypeMultiSelect();
            model.CarMakes = this.BuildCarMakesSelect();
            model.YearList = this.BuildYearSelect();
        }

        // Selects

        private IEnumerable<SelectListItem> BuildRaceTypeMultiSelect()
        {
            var raceTypes = this.GetRaceTypes()
                .Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                });

            return raceTypes;
        }

        private IEnumerable<SelectListItem> BuildCarMakesSelect()
        {
            var carMakes = this.GetCarMakes()
                .Select(m => new SelectListItem()
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                });

            return carMakes;
        }

        private IEnumerable<SelectListItem> BuildYearSelect()
        {
            var yearList = new List<SelectListItem>();

            for (int year = DateTime.Now.Year; year >= GlobalConstants.MIN_CAR_YEAR; year--)
            {
                string strYear = year.ToString();

                yearList.Add(new SelectListItem()
                {
                    Value = strYear,
                    Text = strYear
                });
            }

            return yearList;
        }

        private ICollection<RaceType> ParseRaceTypes(IEnumerable<string> types)
        {
            // The race types cannot be cached here since the Entity Change Tracker
            // will differ from the session one => exception
            var raceTypes = this.data.RaceTypes.All();
            var carRaceTypes = new HashSet<RaceType>();

            foreach (string enumId in types)
            {
                int id;

                if (int.TryParse(enumId, out id))
                {
                    foreach (RaceType type in raceTypes)
                    {
                        if (type.Id == id)
                        {
                            carRaceTypes.Add(type);
                        }
                    }
                }
            }

            return carRaceTypes;
        }

        // Cache

        private IEnumerable<RaceType> GetRaceTypes()
        {
            var raceTypes = this.data.RaceTypes.All();

            return this.RaceTypesCacheService.Get(raceTypes, 60);
        }

        private IEnumerable<CarMake> GetCarMakes()
        {
            var carMakes = this.data.CarMakes.All();

            return this.CarMakesCacheService.Get(carMakes, 60);
        }

        #endregion
    }
}
