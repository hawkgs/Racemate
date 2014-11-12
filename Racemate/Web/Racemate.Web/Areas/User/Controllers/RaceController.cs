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
            int pageParam = Paging.GetCurrentPage(page);
            var races = this.data.Races.All();
            bool isDescending = false;

            // Ugly as fuck
            switch (sortBy) {
                case "name":
                    if (order == "desc")
                    {
                        races = races.OrderByDescending(r => r.Name);
                    }
                    else
                    {
                        races = races.OrderBy(r => r.Name);
                        isDescending = true;
                    }
                    break;
                default:
                    races = races.OrderByDescending(r => r.CreatedOn);
                    break;
            }
            
            var modelRaces = races.Skip(pageParam * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .Project().To<RaceGridDetailsViewModel>();

            int pageCount = Paging.GetPageCount(races.Count(), PAGE_SIZE);

            return this.View(new RaceListViewModel() { 
                Collection = modelRaces,
                PageCount = pageCount,
                CurrentPage = pageParam + 1,
                IsDescending = isDescending,
                SortBy = sortBy,
                Order = order
            });
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
    }
}