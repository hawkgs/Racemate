using Racemate.Data;
using Racemate.Web.Controllers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Generators;
using Racemate.Data.Models;
using Racemate.Web.Areas.User.ViewModels.Invitations;
using AutoMapper.QueryableExtensions;

namespace Racemate.Web.Areas.User.Controllers
{
    [Authorize]
    public class InvitationsController : BaseController
    {
        public InvitationsController(IRacemateData data)
            : base(data)
        {
        }

        public ActionResult Index(int? page)
        {
            int pageParam = this.GetPage(page);

            if (this.TempData["ViewData"] != null)
            {
                this.ViewData = (ViewDataDictionary)this.TempData["ViewData"];
            }

            // Custom mapping
            AutoMapper.Mapper.CreateMap<InvitationCode, InvitationCodeViewModel>()
                .ForMember(dest => dest.User,
                           opts => opts.MapFrom(src => src.User.UserName));

            var invitationCodes = this.data.InvitationCodes.All()
                .Where(i => i.CreatorId == this.CurrentUser.Id);

            var mappedCodes = invitationCodes
                .OrderByDescending(i => i.CreatedOn)
                .Skip(pageParam * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .Project().To<InvitationCodeViewModel>();

            int pageCount = invitationCodes
                .Count() / PAGE_SIZE;

            return View(new InvitationsViewModel() {
                Codes = mappedCodes,
                PageCount = pageCount,
                CurrentPage = pageParam + 1
            });
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