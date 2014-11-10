using Racemate.Data;
using Racemate.Web.Controllers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Generators;
using Racemate.Data.Models;

namespace Racemate.Web.Areas.User.Controllers
{
    [Authorize]
    public class InvitationsController : BaseController
    {
        public InvitationsController(IRacemateData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            if (this.TempData["ViewData"] != null)
            {
                this.ViewData = (ViewDataDictionary)this.TempData["ViewData"];
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateCode()
        {
            bool areThereMoreThan3Codes = this.data.InvitationCodes.All()
                .Where
                (
                    i => i.CreatorId == this.CurrentUser.Id &&
                         i.UserId == null
                )
                .Count() >= 3;

            if (areThereMoreThan3Codes)
            {
                this.ModelState.AddModelError("", "You cannot have more than 3 unused invitation codes!");
                this.TempData["ViewData"] = this.ViewData;

                return RedirectToAction("Index");
            }

            string code;

            do
            {
                code = InvitationCodeGenerator.Generate();
            }
            while (this.data.InvitationCodes.All().Any(i => i.Code == code));

            var invitationCode = new InvitationCode()
            {
                Code = code,
                Creator = this.CurrentUser
            };

            this.data.InvitationCodes.Add(invitationCode);
            this.data.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}