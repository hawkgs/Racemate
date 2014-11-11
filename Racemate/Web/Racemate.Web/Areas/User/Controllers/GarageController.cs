using Racemate.Data;
using Racemate.Data.Models;
using Racemate.Web.Areas.User.ViewModels.Garage;
using Racemate.Web.Controllers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using System.Web.Caching;
using AutoMapper;

namespace Racemate.Web.Areas.User.Controllers
{
    [Authorize]
    public class GarageController : BaseController
    {
        private const int MIN_CAR_YEAR = 1925;

        public GarageController(IRacemateData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            var cars = this.data.Cars.All()
                .Where(c => c.OwnerId == this.CurrentUser.Id)
                .Project().To<CarViewModel>();

            return this.View(cars);
        }

        public ActionResult AddCar()
        {
            var model = new AddCarViewModel()
            {
                RaceTypesList = this.BuildRaceTypeMultiSelect(),
                CarMakes = this.GetCarMakes(),
                MinimalCarYear = MIN_CAR_YEAR
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCar(AddCarViewModel model)
        {
            int carModelId;
            int year;
            CarModel carModel = null;
            var raceTypes = this.ParseRaceTypes(model.SelectedRaceTypes);

            if (int.TryParse(this.Request["CarModelId"], out carModelId))
            {
                carModel = this.data.CarModels.All()
                    .FirstOrDefault(c => c.Id == carModelId);
            }

            // TODO: Fix that shit
            if (carModel == null)
            {
                this.ModelState.AddModelError("", "The car make and/or model are invalid!");

                this.AttachContentToModel(model);
                return this.View(model);
            }
            else if (raceTypes.Count == 0)
            {
                this.ModelState.AddModelError("", "The selected race type(s) are invalid!");

                this.AttachContentToModel(model);
                return this.View(model);
            }
            else if (!int.TryParse(this.Request["YearOfProduction"], out year))
            {
                this.ModelState.AddModelError("", "The provided year is invalid!");

                this.AttachContentToModel(model);
                return this.View(model);
            }
            else if (!this.ModelState.IsValid)
            {
                this.AttachContentToModel(model);
                return this.View(model);
            }

            var car = Mapper.Map<AddCarViewModel, Car>(model);

            car.Year = year.ToString();            
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
            model.CarMakes = this.GetCarMakes();
        }

        private ICollection<RaceType> ParseRaceTypes(IEnumerable<string> types)
        {
            var raceTypes = this.GetRaceTypes();
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

        private IEnumerable<SelectListItem> BuildRaceTypeMultiSelect()
        {
            var raceTypes = this.data.RaceTypes.All();
            var raceTypesList = new List<SelectListItem>();

            foreach (var raceType in raceTypes)
            {
                raceTypesList.Add(new SelectListItem()
                {
                    Text = raceType.Name,
                    Value = raceType.Id.ToString()
                });
            }

            return raceTypesList;
        }

        private IQueryable<RaceType> GetRaceTypes()
        {
            const int CACHE_HR = 24;

            if (this.HttpContext.Cache["raceTypes"] == null)
            {
                var raceTypes = this.data.RaceTypes.All();

                this.HttpContext.Cache.Insert(
                "raceTypes",
                raceTypes,
                null,
                DateTime.Now.AddHours(CACHE_HR),
                TimeSpan.Zero,
                CacheItemPriority.Default,
                this.OnCacheItemRemovedCallback);
            }

            return (IQueryable<RaceType>)this.HttpContext.Cache["raceTypes"];
        }

        private IQueryable<CarMake> GetCarMakes()
        {
            const int CACHE_MIN = 30;

            if (this.HttpContext.Cache["carMakes"] == null)
            {
                var carMakes = this.data.CarMakes.All();

                this.HttpContext.Cache.Insert(
                "carMakes",
                carMakes,
                null,
                DateTime.Now.AddMinutes(CACHE_MIN),
                TimeSpan.Zero,
                CacheItemPriority.Default,
                this.OnCacheItemRemovedCallback);
            }

            return (IQueryable<CarMake>)this.HttpContext.Cache["carMakes"];
        }

        private void OnCacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            // Cache item removed
        }

        #endregion
    }
}