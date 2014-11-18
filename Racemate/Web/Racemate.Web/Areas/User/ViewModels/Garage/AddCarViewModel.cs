namespace Racemate.Web.Areas.User.ViewModels.Garage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using AutoMapper;
    using Racemate.Common;
    using Racemate.Data.Models;
    using Racemate.Web.Infrastructure.Mapping;

    public class AddCarViewModel : IMapFrom<Car>, IHaveCustomMappings
    {
        private const string RNG_ERR_MSG = "The {0} of the vehicle must be between {1} and {2}.";
        private const string REQ_ERR_MSG = "{0} is required.";
        private const int YEAR = 2014;

        [Display(Name = "Type(s)")]
        public IEnumerable<SelectListItem> RaceTypesList { get; set; }

        public IEnumerable<SelectListItem> YearList { get; set; }

        public IEnumerable<SelectListItem> CarMakes { get; set; }

        public IEnumerable<string> SelectedRaceTypes { get; set; }

        [Display(Name = "Weight")]
        [Range(250, 5500, ErrorMessage = RNG_ERR_MSG)]
        [Required(ErrorMessage = REQ_ERR_MSG)]
        public int Weight { get; set; }

        [Display(Name = "Horsepower")]
        [Range(20, 3000, ErrorMessage = RNG_ERR_MSG)]
        [Required(ErrorMessage = REQ_ERR_MSG)]
        public int Hp { get; set; }

        [Display(Name = "Torque")]
        [Range(20, 3000, ErrorMessage = RNG_ERR_MSG)]
        [Required(ErrorMessage = REQ_ERR_MSG)]
        public int Torque { get; set; }

        [Display(Name = "Engine Aspiration")]
        [Required(ErrorMessage = REQ_ERR_MSG)]
        public EngineAspiration EngineAspiration { get; set; }

        [Display(Name = "Year Of Production")]
        [Range(GlobalConstants.MIN_CAR_YEAR, YEAR)]
        [Required(ErrorMessage = REQ_ERR_MSG)]
        public int Year { get; set; }

        [Display(Name = "Engine Displacement")]
        [Range(50, 7000, ErrorMessage = RNG_ERR_MSG)]
        public int? EngineDisplacement { get; set; }

        [Display(Name = "ALS (Antilag System)")]
        public bool IsAntilagMounted { get; set; }

        [Display(Name = "Launch Control")]
        public bool IsLaunchControlMounted { get; set; }

        [Display(Name = "N2O System")]
        public bool IsN2oMounted { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Car, AddCarViewModel>()
                .ForMember(dest => dest.Year,
                           opts => opts.MapFrom(src => src.Year.ToString()))
                           .ReverseMap();
        }
    }
}