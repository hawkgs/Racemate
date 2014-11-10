using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Racemate.Web.Controllers.Common;
using Racemate.Data;

namespace Racemate.Web.Areas.Users.Controllers
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
            var test = this.data.InvitationCodes.All().ToList();

            return View();
        }
    }
}