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
            return View();
        }

        public ActionResult AddCar()
        {
            var model = new AddCarViewModel()
            {
                RaceTypesList = this.BuildRaceTypeMultiSelect(),
                CarMakes = this.GetCarMakes(),
                MinimalCarYear = MIN_CAR_YEAR
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCar(AddCarViewModel model)
        {
            var carModel = this.Request["CarModelId"];

            if (!this.ModelState.IsValid)
            {
                model.RaceTypesList = this.BuildRaceTypeMultiSelect();
                model.CarMakes = this.GetCarMakes();
                this.ModelState.AddModelError("", "Yasdasd");

                return View(model);
            }

            return RedirectToAction("Index");
        }

        public JsonResult MakeModels(int makeId)
        {
            var models = this.data.CarModels.All()
                .Where(m => m.CarMakeId == makeId)
                .Project().To<CarModelViewModel>();

            return this.Json(models, JsonRequestBehavior.AllowGet);
        }

        #region Helpers

        private IEnumerable<SelectListItem> BuildRaceTypeMultiSelect()
        {
            var raceTypes = Enum.GetValues(typeof(RaceType)).Cast<RaceType>().ToList();
            var raceTypesList = new List<SelectListItem>();

            foreach (var enumItem in raceTypes)
            {
                raceTypesList.Add(new SelectListItem()
                {
                    Text = enumItem.ToString(),
                    Value = ((int)enumItem).ToString()
                });
            }

            return raceTypesList;
        }

        private IQueryable<CarMake> GetCarMakes()
        {
            if (this.HttpContext.Cache["carMakes"] == null)
            {
                var carMakes = this.data.CarMakes.All();

                this.HttpContext.Cache.Insert(
                "carMakes",
                carMakes,
                null,
                DateTime.Now.AddMinutes(15),
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