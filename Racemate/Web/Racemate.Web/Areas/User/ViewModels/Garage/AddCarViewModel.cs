namespace Racemate.Web.Areas.User.ViewModels.Garage
{
    using System.Collections.Generic;
    using Racemate.Data.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Racemate.Web.Infrastructure.Mapping;
    using AutoMapper;

    public class AddCarViewModel : IMapFrom<Car>, IHaveCustomMappings
    {
        private const string RNG_ERR_MSG = "The {0} of the vehicle must be between {1} and {2}.";
        private const string REQ_ERR_MSG = "{0} is required.";

        [Display(Name = "Type(s)")]
        public IEnumerable<SelectListItem> RaceTypesList { get; set; }

        public IEnumerable<string> SelectedRaceTypes { get; set; }

        public IEnumerable<CarMake> CarMakes { get; set; }

        public int MinimalCarYear { get; set; }

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
        public int? Year { get; set; }

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