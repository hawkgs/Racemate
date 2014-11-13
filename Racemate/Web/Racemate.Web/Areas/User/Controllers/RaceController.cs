using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Racemate.Web.Controllers.Common;
using Racemate.Data;
using Racemate.Web.Areas.User.ViewModels.Race;
using System.Net;
using AutoMapper;
using Racemate.Data.Models;
using System.Data.Entity.Validation;
using AutoMapper.QueryableExtensions;
using Racemate.Common;
using Racemate.Web.Areas.User.ViewModels.Home;
using Racemate.Common.Contracts;

namespace Racemate.Web.Areas.User.Controllers
{
    [Authorize]
    public class RaceController : BaseController
    {
        public RaceController(IRacemateData data)
            : base(data)
        {
        }

        public ActionResult List(int? page, string sortBy, string order)
        {
            var races = this.data.Races.All();

            return this.RaceList(races, page, sortBy, order);
        }

        public ActionResult MyRaces(int? page, string sortBy, string order)
        {
            var races = this.data.Races.All()
                .Where(r => r.OrganizerId == this.CurrentUser.Id);

            return this.RaceList(races, page, sortBy, order);
        }

        public ActionResult Create()
        {
            ViewBag.RaceTypesSelect = this.data.RaceTypes.All()
                .Select(r => new SelectListItem() { 
                    Text = r.Name,
                    Value = r.Id.ToString()
                });

            return this.View();
        }

        [HttpPost]
        public JsonResult Create(RaceInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = this.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(errors);
            }

            var raceType = this.data.RaceTypes.All()
                .FirstOrDefault(r => r.Id == model.TypeId);

            if (raceType == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return this.Json(new string[] { "The type field is invalid!" });
            }
            else if (DateTime.Now > model.DateTimeOfRace)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return this.Json(new string[] { "The date must be in future!" });
            }

            // Everything should be OK after this line
            model.Type = raceType;

            var race = Mapper.Map<RaceInputModel, Race>(model);
            race.Organizer = this.CurrentUser;

            this.data.Races.Add(race);
            this.data.SaveChanges();

            return this.Json(new { });
        }

        [NonAction]
        private ActionResult RaceList(IQueryable<Race> races, int? page, string sortBy, string order)
        {
            int pageParam = Paging.GetCurrentPage(page);
            bool isDescending = false;
            ISorter<Race> sorter = new Sorter<Race>(races);

            // Ugly as fuck
            switch (sortBy)
            {
                case "name":
                    isDescending = sorter.SortBy(order, isDescending, r => r.DateTimeOfRace);
                    break;
                case "date":
                    isDescending = sorter.SortBy(order, isDescending, r => r.DateTimeOfRace);
                    break;
                case "type":
                    isDescending = sorter.SortBy(order, isDescending, r => r.Type.Name);
                    break;
                case "positions":
                    isDescending = sorter.SortBy(order, isDescending, r => r.AvailableRacePositions - r.Participants.Count);
                    break;
                default:
                    sorter.Collection = sorter.Collection.OrderByDescending(r => r.CreatedOn);
                    break;
            }

            var modelRaces = sorter.Collection.Skip(pageParam * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .Project().To<RaceThumbViewModel>();

            int pageCount = Paging.GetPageCount(races.Count(), PAGE_SIZE);

            return this.View(new RaceListViewModel()
            {
                Collection = modelRaces,
                PageCount = pageCount,
                CurrentPage = pageParam + 1,
                IsDescending = isDescending,
                SortBy = sortBy,
                Order = order
            });
        }
    }
}