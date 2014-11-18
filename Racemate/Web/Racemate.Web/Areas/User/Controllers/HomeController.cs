namespace Racemate.Web.Areas.User.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Racemate.Data;
    using Racemate.Web.Areas.User.ViewModels.Home;
    using Racemate.Web.Controllers.Common;
    using Racemate.Web.Infrastructure.Caching;
    using Racemate.Web.Infrastructure.Caching.Contracts;

    [Authorize]
    public class HomeController : BaseController
    {
        private const int LATEST_RACES_NUM = 5;
        private const int TOP_MEMBERS_NUM = 8;

        public HomeController(IRacemateData data)
            : base(data)
        {
            this.HomeMapCache = new CacheService<RaceMapDataModel>("homeMap");
            this.TopMembersCache = new CacheService<UserThumbViewModel>("topMembers");
        }

        public ICacheService<RaceMapDataModel> HomeMapCache { get; private set; }

        public ICacheService<UserThumbViewModel> TopMembersCache { get; private set; }

        public ActionResult Index()
        {
            var latestRaces = this.data.Races.All()
                .OrderByDescending(r => r.CreatedOn)
                .Take(LATEST_RACES_NUM)
                .Project().To<RaceThumbViewModel>();

            var topMembers = this.GetTopMembers();

            var model = new HomeViewModel()
            {
                LatestRaces = latestRaces,
                TopMembers = topMembers
            };

            return this.View(model);
        }

        public JsonResult MapRaces()
        {
            var mapRaces = this.GetAllMapRaces();

            return this.Json(mapRaces, JsonRequestBehavior.AllowGet);
        }

        #region Helpers

        private IEnumerable<RaceMapDataModel> GetAllMapRaces()
        {
            var allRaces = this.data.Races.All()
                .Where(r =>
                    (!r.IsFinished && !r.IsCanceled) &&
                    (r.DateTimeOfRace > DateTime.Now) ||
                    (r.DateTimeOfRace < DateTime.Now) // TODO during race
                )
                .Project().To<RaceMapDataModel>();

            return this.HomeMapCache.Get(allRaces, 1);
        }

        private IEnumerable<UserThumbViewModel> GetTopMembers()
        {
            var topMembers = this.data.Users.All()
                .OrderByDescending(u => u.FirstPlaces)
                .ThenByDescending(u => u.SecondPlaces)
                .ThenByDescending(u => u.ThirdPlaces)
                .Take(TOP_MEMBERS_NUM)
                .Project().To<UserThumbViewModel>();

            return this.TopMembersCache.Get(topMembers, 60);
        }

        #endregion
    }
}