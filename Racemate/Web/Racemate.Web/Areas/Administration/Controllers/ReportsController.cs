namespace Racemate.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}