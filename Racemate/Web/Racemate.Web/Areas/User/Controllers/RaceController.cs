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

namespace Racemate.Web.Areas.User.Controllers
{
    [Authorize]
    public class RaceController : BaseController
    {
        public RaceController(IRacemateData data)
            : base(data)
        {
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
        public JsonResult Create(RaceDataModel model)
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

                return Json(new string[] {"The type field is invalid!"});
            }
            else if (DateTime.Now > model.DateTimeOfRace)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(new string[] { "The date must be in future!" });
            }

            // Everything should be OK after this line
            model.Type = raceType;

            var car = Mapper.Map<RaceDataModel, Race>(model);
            car.Organizer = this.CurrentUser;

            this.data.Races.Add(car);
            this.data.SaveChanges();

            return Json(new { });
        }

        #region Helper



        #endregion
    }
}