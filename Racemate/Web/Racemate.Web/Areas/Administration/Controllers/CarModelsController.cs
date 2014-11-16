namespace Racemate.Web.Areas.Administration.Controllers
{
    using System.Collections;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Kendo.Mvc.UI;
    using Racemate.Data;
    using Racemate.Web.Areas.Administration.Controllers.Common;
    using Racemate.Web.Areas.Administration.ViewModels.CarModels;

    using Model = Racemate.Data.Models.CarModel;
    using ViewModel = Racemate.Web.Areas.Administration.ViewModels.CarModels.CarModelViewModel;

    public class CarModelsController : KendoGridAdministrationController
    {
        public CarModelsController(IRacemateData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            this.PopulateMakes();

            return this.View();
        }

        protected override IEnumerable GetData()
        {
            return this.data.CarModels.All()
                .Project().To<CarModelViewModel>();
        }

        protected override T GetById<T>(object id)
        {
            return this.data.CarMakes.GetById(id) as T;
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, ViewModel model)
        {
            var dbModel = base.Create<Model>(model);
            if (dbModel != null)
            {
                model.Id = dbModel.Id;
            }

            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, ViewModel model)
        {
            base.Update<Model, ViewModel>(model, model.Id);

            return this.GridOperation(model, request);
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, ViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                //this.data.CarMakes.Delete(model.Id);
                this.data.SaveChanges();
            }

            return this.GridOperation(model, request);
        }

        #region Helpers

        // TODO: Cache
        private void PopulateMakes()
        {
            var makes = this.data.CarMakes.All()
                .Select(m => new SelectListItem()
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                });

            this.ViewBag.Makes = makes;
        }

        #endregion
    }
}