namespace Racemate.Web.Areas.User.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Generators;

    using Racemate.Data;
    using Racemate.Data.Models;
    using Racemate.Web.Areas.User.ViewModels.Invitations;
    using Racemate.Web.Controllers.Common;
    using Racemate.Common;

    [Authorize]
    public class InvitationsController : BaseController
    {
        private const int MAX_UNUSED_CODES = 3;

        public InvitationsController(IRacemateData data)
            : base(data)
        {
        }

        public ActionResult Index(int? page)
        {
            int pageParam = Paging.GetCurrentPage(page);

            if (this.TempData["ViewData"] != null)
            {
                this.ViewData = (ViewDataDictionary)this.TempData["ViewData"];
            }

            var invitationCodes = this.data.InvitationCodes.All()
                .Where(i => i.CreatorId == this.CurrentUser.Id);

            var mappedCodes = invitationCodes
                .OrderByDescending(i => i.CreatedOn)
                .Skip(pageParam * GlobalConstants.PAGE_SIZE)
                .Take(GlobalConstants.PAGE_SIZE)
                .Project().To<InvitationCodeViewModel>();

            int pageCount = Paging.GetPageCount(invitationCodes.Count(), GlobalConstants.PAGE_SIZE);

            return this.View(new InvitationsViewModel() {
                Collection = mappedCodes,
                PageCount = pageCount,
                CurrentPage = pageParam + 1
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateCode()
        {
            bool areThereCodesWithinLimit = this.data.InvitationCodes.All()
                .Where
                (
                    i => i.CreatorId == this.CurrentUser.Id &&
                         i.UserId == null
                )
                .Count() >= MAX_UNUSED_CODES;

            if (areThereCodesWithinLimit)
            {
                this.ModelState.AddModelError("", "You cannot have more than 3 unused invitation codes!");
                this.TempData["ViewData"] = this.ViewData;

                return this.RedirectToAction("Index");
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

            return this.RedirectToAction("Index");
        }
    }
}