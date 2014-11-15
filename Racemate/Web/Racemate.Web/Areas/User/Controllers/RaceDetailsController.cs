namespace Racemate.Web.Areas.User.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;

    using Racemate.Common;
    using Racemate.Data;
    using Racemate.Data.Models;
    using Racemate.Web.Areas.User.ViewModels.Race;
    using Racemate.Web.Controllers.Common;
    using Racemate.Web.Infrastructure.Validators;

    [Authorize]
    public class RaceDetailsController : BaseController
    {
        public RaceDetailsController(IRacemateData data)
            : base(data)
        {
            this.Validator = new RaceDetailsValidator(this.ModelState, this.TempData, this.ViewData);
        }

        public RaceDetailsValidator Validator { get; private set; }

        public ActionResult Details(string id)
        {
            if (this.TempData["ViewData"] != null)
            {
                this.ViewData = (ViewDataDictionary)this.TempData["ViewData"];
            }

            int raceId;
            if (!this.IsRaceIdValid(id, out raceId))
            {
                return this.RedirectToList();
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
                return this.RedirectToList();
            }

            var raceCar = this.CurrentUser.Cars
                .FirstOrDefault(c => c.Id == model.UserRaceCarId);

            if (raceCar == null)
            {
                this.Validator.NoSuchCarError();
                return this.RedirectToRace(model.EncryptedId);
            }

            var race = this.data.Races.GetById(raceId);

            if (DateTime.Now > race.DateTimeOfRace)
            {
                this.Validator.RaceHasStartedError();
                return this.RedirectToRace(model.EncryptedId);
            }

            int participantsCount = race.Participants
                .Where(p => !p.IsDeleted && !p.IsKicked)
                .Count();

            if (participantsCount == race.AvailableRacePositions)
            {
                this.Validator.NoAvailableRacePositionsError();
                return this.RedirectToRace(model.EncryptedId);
            }

            var participant = new RaceParticipant()
            {
                User = this.CurrentUser,
                Car = raceCar
            };
            race.Participants.Add(participant);

            return this.SaveAndRedirect(model.EncryptedId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Spectate(RaceDetailsViewModel model)
        {
            int raceId;
            if (!this.IsRaceIdValid(model.EncryptedId, out raceId))
            {
                return this.RedirectToList();
            }

            var race = this.data.Races.GetById(raceId);

            if (DateTime.Now > race.DateTimeOfRace)
            {
                this.Validator.RaceHasStartedError();
                return this.RedirectToRace(model.EncryptedId);
            }

            var spectator = new RaceSpectator();
            spectator.User = this.CurrentUser;
            race.Spectators.Add(spectator);

            return this.SaveAndRedirect(model.EncryptedId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(RaceDetailsViewModel model)
        {
            int raceId;
            if (!this.IsRaceIdValid(model.EncryptedId, out raceId))
            {
                return this.RedirectToList();
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

            return this.SaveAndRedirect(model.EncryptedId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Finish(RaceDetailsViewModel model)
        {
            int raceId;
            if (!this.IsRaceIdValid(model.EncryptedId, out raceId))
            {
                return this.RedirectToList();
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

            return this.SaveAndRedirect(model.EncryptedId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Leave(RaceDetailsViewModel model)
        {
            int raceId;
            if (!this.IsRaceIdValid(model.EncryptedId, out raceId))
            {
                return this.RedirectToList();
            }

            var race = this.data.Races.GetById(raceId);

            if (DateTime.Now > race.DateTimeOfRace)
            {
                // race has already started
            }

            int participantId = race.Participants
                .FirstOrDefault(p => p.UserId == this.CurrentUser.Id && !p.IsDeleted && !p.IsKicked).Id;

            this.data.RaceParticipants.Delete(participantId);

            return this.SaveAndRedirect(model.EncryptedId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kick(RaceDetailsViewModel model)
        {
            int raceId;
            if (!this.IsRaceIdValid(model.EncryptedId, out raceId))
            {
                return this.RedirectToList();
            }

            var race = this.data.Races.GetById(raceId);

            if (DateTime.Now > race.DateTimeOfRace.AddHours(race.Duration))
            {
                // race has already finished
            }

            var participant = race.Participants
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

            return this.SaveAndRedirect(model.EncryptedId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Positioning(RaceDetailsViewModel model)
        {
            int raceId;
            if (!this.IsRaceIdValid(model.EncryptedId, out raceId))
            {
                return this.RedirectToList();
            }

            // validation

            var participants = this.data.Races.GetById(raceId).Participants
                .Where(p => !p.IsDeleted && !p.IsKicked);

            foreach (var participantData in model.Participants)
            {
                var participant = participants
                    .FirstOrDefault(p => p.Id == participantData.Id);

                if (participant != null)
                {
                    if (participantData.FinishTime != null)
                    {
                        participant.FinishTime = participantData.FinishTime;
                    }
                    
                    participant.FinishPosition = participantData.FinishPosition;
                }
            }

            return this.SaveAndRedirect(model.EncryptedId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlertForPolice(RaceDetailsViewModel model)
        {
            int raceId;
            if (!this.IsRaceIdValid(model.EncryptedId, out raceId))
            {
                return this.RedirectToList();
            }

            var race = this.data.Races.GetById(raceId);

            if (DateTime.Now > race.DateTimeOfRace.AddHours(race.Duration))
            {
                // race has already finished
            }

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

            return this.SaveAndRedirect(model.EncryptedId);
        }

        #region Helpers
        
        [NonAction]
        private ActionResult RedirectToList()
        {
            return this.RedirectToAction("List", "Race");
        }

        [NonAction]
        private ActionResult RedirectToRace(string encryptedId)
        {
            return this.RedirectToAction("Details", new { id = encryptedId });
        }

        [NonAction]
        private ActionResult SaveAndRedirect(string encryptedId)
        {
            this.data.SaveChanges();

            return this.RedirectToAction("Details", new { id = encryptedId });
        }

        private bool IsRaceIdValid(string encryptedId, out int raceId)
        {
            string decryptedId = QueryStringBuilder.DecryptRaceId(encryptedId);

            return int.TryParse(decryptedId, out raceId);
        }

        #endregion
    }
}