namespace Racemate.Web.Controllers
{
    using System.Web.Mvc;
    using Racemate.Web.Controllers.Common;
    using Racemate.Data;

    public class ErrorController : BaseController
    {
        public ErrorController(IRacemateData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View("Error");
        }
    }
}