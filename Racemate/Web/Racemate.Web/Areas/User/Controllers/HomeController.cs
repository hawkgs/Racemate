using Racemate.Data;
using Racemate.Web.Areas.User.ViewModels.Home;
using Racemate.Web.Controllers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using model = Racemate.Data.Models;
using System.Web.Caching;
using Racemate.Data.Models;

namespace Racemate.Web.Areas.User.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private const int LATEST_RACES_NUM = 5;
        private const int TOP_MEMBERS_NUM = 8;

        public HomeController(IRacemateData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            var latestRaces = this.data.Races.All()
                .OrderByDescending(r => r.CreatedOn)
                .Take(LATEST_RACES_NUM)
                .Project().To<RaceThumbViewModel>();

            var topMembers = this.GetTopMembers()
                .Take(TOP_MEMBERS_NUM)
                .Project().To<UserThumbViewModel>();

            var model = new HomeViewModel()
            {
                LatestRaces = latestRaces,
                TopMembers = topMembers
            };

            return this.View(model);
        }

        public JsonResult MapRaces()
        {
            var mapRaces = this.GetAllMapRaces()
                .Project().To<RaceMapDataModel>();

            return this.Json(mapRaces, JsonRequestBehavior.AllowGet);
        }

        #region Helpers

        private IQueryable<Race> GetAllMapRaces()
        {
            const int CACHE_MIN = 1;
            const string KEY = "allRaces";

            if (this.HttpContext.Cache[KEY] == null)
            {
                var allRaces = this.data.Races.All()
                    .Where(r => 
                        (!r.IsFinished && !r.IsCanceled) &&
                        (r.DateTimeOfRace > DateTime.Now) ||
                        (r.DateTimeOfRace < DateTime.Now)
                    );

                this.HttpContext.Cache.Insert(
                KEY,
                allRaces,
                null,
                DateTime.Now.AddMinutes(CACHE_MIN),
                TimeSpan.Zero,
                CacheItemPriority.Default,
                this.OnCacheItemRemovedCallback);
            }

            return (IQueryable<Race>)this.HttpContext.Cache[KEY];
        }

        private IQueryable<model.User> GetTopMembers()
        {
            const int CACHE_MIN = 60;
            const string KEY = "topMembers";

            if (this.HttpContext.Cache[KEY] == null)
            {
                var topMembers = this.data.Users.All()
                    .OrderByDescending(u => u.FirstPlaces)
                    .ThenByDescending(u => u.SecondPlaces)
                    .ThenByDescending(u => u.ThirdPlaces)
                    .Take(TOP_MEMBERS_NUM);

                this.HttpContext.Cache.Insert(
                KEY,
                topMembers,
                null,
                DateTime.Now.AddMinutes(CACHE_MIN),
                TimeSpan.Zero,
                CacheItemPriority.Default,
                this.OnCacheItemRemovedCallback);
            }

            return (IQueryable<model.User>)this.HttpContext.Cache[KEY];
        }

        private void OnCacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            // Cache item removed
        }

        #endregion
    }
}