using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Racemate.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}