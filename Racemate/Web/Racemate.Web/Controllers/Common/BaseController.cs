namespace Racemate.Web.Controllers.Common
{
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNet.Identity;
    using Racemate.Data;
    using Racemate.Data.Models;
    using Racemate.Web.Areas.User.ViewModels.Common;

    public class BaseController : Controller
    {
        protected readonly IRacemateData data;

        protected virtual User CurrentUser
        {
            get
            {
                return this.data.Users.All()
                    .FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            }
        }

        public BaseController(IRacemateData data)
        {
            this.data = data;
        }

        public JsonResult Notifications()
        {
            var notifications = this.data.Users
                .GetById(this.User.Identity.GetUserId())
                .Notifications
                .OrderByDescending(n => n.CreatedOn)
                .Take(5)
                .AsQueryable()
                .Project().To<NotificationViewModel>();

            return Json(notifications, JsonRequestBehavior.AllowGet);
        }
    }
}