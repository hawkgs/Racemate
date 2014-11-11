using Racemate.Data;
using Racemate.Web.Controllers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Racemate.Web.Areas.User.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(IRacemateData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }
    }
}