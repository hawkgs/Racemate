namespace Racemate.Web.Areas.User.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Newtonsoft.Json;

    using Racemate.Common;
    using Racemate.Common.Contracts;
    using Racemate.Data;
    using Racemate.Data.Models;
    using Racemate.Web.Areas.User.ViewModels.Home;
    using Racemate.Web.Areas.User.ViewModels.Race;
    using Racemate.Web.Controllers.Common;

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

        public ActionResult Details(string id)
        {
            int raceId;
            if (!this.IsRaceIdValid(id, out raceId))
            {
                return this.RedirectToAction("List");
            }

            var race = this.data.Races.GetById(raceId);
            var model = Mapper.Map<Race, RaceDetailsViewModel>(race);

            var userVehicles = this.CurrentUser.Cars
                .Where(c => c.RaceTypes.Any(t => t.Name == model.Type))
                .Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = String.Format("{0} {1}", c.Model.CarMake.Name, c.Model.Name)
                });

            if (race.OrganizerId == this.CurrentUser.Id)
            {
                var kickUserSelect = race.Participants
                    .Where(p => !p.IsKicked && !p.IsDeleted && p.UserId != this.CurrentUser.Id)
                    .Select(p => new SelectListItem()
                    {
                        Value = p.UserId.ToString(),
                        Text = p.User.UserName
                    });

                model.KickUserSelect = kickUserSelect;
            }

            model.EncryptedId = id;
            model.UserCarSelect = userVehicles;

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join(RaceDetailsViewModel model)
        {
            int raceId;
            if (!this.IsRaceIdValid(model.EncryptedId, out raceId))
            {
                return this.RedirectToAction("List");
            }

            var raceCar = this.CurrentUser.Cars
                .FirstOrDefault(c => c.Id == model.UserRaceCarId);

            if (raceCar == null)
            {
                // error
            }

            var race = this.data.Races.GetById(raceId);
            var participant = new RaceParticipant()
            {
                User = this.CurrentUser,
                Car = raceCar
            };

            if (race.Participants.Count == race.AvailableRacePositions)
            {
                // no free places
            }

            race.Participants.Add(participant);
            this.data.SaveChanges();

            return this.RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Spectate(RaceDetailsViewModel model)
        {
            int raceId;
            if (!this.IsRaceIdValid(model.EncryptedId, out raceId))
            {
                return this.RedirectToAction("List");
            }

            var race = this.data.Races.GetById(raceId);

            var spectator = new RaceSpectator();
            spectator.User = this.CurrentUser;
            race.Spectators.Add(spectator);

            this.data.SaveChanges();

            return this.RedirectToAction("Details", new { id = model.EncryptedId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(RaceDetailsViewModel model)
        {
            int raceId;
            if (!this.IsRaceIdValid(model.EncryptedId, out raceId))
            {
                return this.RedirectToAction("List");
            }

            var race = this.data.Races.GetById(raceId);

            if (race.OrganizerId != this.CurrentUser.Id)
            {
                // not an organizer
            }

            if (DateTime.Now > race.DateTimeOfRace)
            {
                // race has already started
            }

            race.IsCanceled = true;
            this.data.SaveChanges();

            return this.RedirectToAction("Details", new { id = model.EncryptedId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Finish(RaceDetailsViewModel model)
        {
            int raceId;
            if (!this.IsRaceIdValid(model.EncryptedId, out raceId))
            {
                return this.RedirectToAction("List");
            }

            var race = this.data.Races.GetById(raceId);

            if (race.OrganizerId != this.CurrentUser.Id)
            {
                // not an organizer
            }

            if (DateTime.Now > race.DateTimeOfRace.AddHours(race.Duration))
            {
                // race has already finished
            }

            race.IsFinished = true;
            this.data.SaveChanges();

            return this.RedirectToAction("Details", new { id = model.EncryptedId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Leave(RaceDetailsViewModel model)
        {
            int raceId;
            if (!this.IsRaceIdValid(model.EncryptedId, out raceId))
            {
                return this.RedirectToAction("List");
            }

            int participantId = this.data.Races.GetById(raceId).Participants
                .FirstOrDefault(p => p.UserId == this.CurrentUser.Id && !p.IsDeleted && !p.IsKicked).Id;

            this.data.RaceParticipants.Delete(participantId);
            this.data.SaveChanges();

            return this.RedirectToAction("Details", new { id = model.EncryptedId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kick(RaceDetailsViewModel model)
        {
            int raceId;
            if (!this.IsRaceIdValid(model.EncryptedId, out raceId))
            {
                return this.RedirectToAction("List");
            }

            var participant = this.data.Races.GetById(raceId).Participants
                .FirstOrDefault(p => 
                    p.UserId == model.KickUserId &&
                    p.UserId != this.CurrentUser.Id && 
                    !p.IsDeleted &&
                    !p.IsKicked);

            if (participant == null)
            {
                // no such user error
            }

            participant.IsKicked = true;
            this.data.SaveChanges();

            return this.RedirectToAction("Details", new { id = model.EncryptedId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Positioning(RaceDetailsViewModel model)
        {
            int raceId;
            if (!this.IsRaceIdValid(model.EncryptedId, out raceId))
            {
                return this.RedirectToAction("List");
            }

            return this.RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlertForPolice(RaceDetailsViewModel model)
        {
            int raceId;
            if (!this.IsRaceIdValid(model.EncryptedId, out raceId))
            {
                return this.RedirectToAction("List");
            }

            var race = this.data.Races.GetById(raceId);

            var participant = race.Participants
                .FirstOrDefault(s => s.UserId == this.CurrentUser.Id);

            if (participant != null)
            {
                participant.IsPoliceAlerted = true;
            }
            else
            {
                var spectator = race.Spectators
                    .FirstOrDefault(s => s.UserId == this.CurrentUser.Id);

                spectator.IsPoliceAlerted = true;
            }

            this.data.SaveChanges();

            return this.RedirectToAction("Details", new { id = model.EncryptedId });
        }

        public JsonResult RaceRoute(string id)
        {
            string decryptedId = QueryStringBuilder.DecryptRaceId(id);
            int raceId;

            if (!int.TryParse(decryptedId, out raceId))
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return this.Json("Invalid race ID", JsonRequestBehavior.AllowGet);
            }

            var race = this.data.Races.GetById(raceId);
            var model = Mapper.Map<Race, RaceRouteDataModel>(race);
            var serialized = JsonConvert.SerializeObject(model);

            return this.Json(serialized, JsonRequestBehavior.AllowGet);
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

            var raceType = this.data.RaceTypes.GetById(model.TypeId);

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
                    isDescending = sorter.SortBy(order, isDescending, r => r.Name);
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

            var modelRaces = sorter.Collection.Skip(pageParam * GlobalConstants.PAGE_SIZE)
                .Take(GlobalConstants.PAGE_SIZE)
                .Project().To<RaceThumbViewModel>();

            int pageCount = Paging.GetPageCount(races.Count(), GlobalConstants.PAGE_SIZE);

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

        #region Helpers

        private bool IsRaceIdValid(string encryptedId, out int raceId)
        {
            string decryptedId = QueryStringBuilder.DecryptRaceId(encryptedId);

            return int.TryParse(decryptedId, out raceId);
        }

        #endregion
    }
}