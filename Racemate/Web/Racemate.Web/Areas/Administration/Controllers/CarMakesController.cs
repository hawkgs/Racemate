namespace Racemate.Web.Areas.Administration.Controllers
{
    using System.Collections;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Kendo.Mvc.UI;
    using Racemate.Data;
    using Racemate.Web.Areas.Administration.Controllers.Common;
    using Racemate.Web.Areas.Administration.ViewModels.CarMakes;

    using Model = Racemate.Data.Models.CarMake;
    using ViewModel = Racemate.Web.Areas.Administration.ViewModels.CarMakes.CarMakeViewModel;

    public class CarMakesController : KendoGridAdministrationController
    {
        public CarMakesController(IRacemateData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }

        protected override IEnumerable GetData()
        {
            return this.data.CarMakes.All()
                .Project().To<CarMakeViewModel>();
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
    }
}