namespace Racemate.Web.Areas.User.ViewModels.Garage
{
    using System.Collections.Generic;
    using Racemate.Data.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Racemate.Web.Infrastructure.Mapping;

    public class AddCarViewModel : IMapFrom<Car>
    {
        [Display(Name = "Type(s)")]
        public IEnumerable<SelectListItem> RaceTypesList { get; set; }

        public IEnumerable<string> SelectedRaceTypes { get; set; }

        public IEnumerable<CarMake> CarMakes { get; set; }

        public int MinimalCarYear { get; set; }

        [Display(Name = "Weight")]
        [Range(250, 5500, ErrorMessage = "The weight of the vehicle must be between {0} and {1} kilograms.")]
        [Required]
        public int Weight { get; set; }

        [Display(Name = "Horsepower")]
        [Range(20, 3000)]
        [Required]
        public int Hp { get; set; }

        [Display(Name = "Torque")]
        [Range(20, 3000)]
        [Required]
        public int Torque { get; set; }

        [Display(Name = "Engine Aspiration")]
        [Required]
        public EngineAspiration EngineAspiration { get; set; }

        [Display(Name = "Year Of Production")]
        public int? Year { get; set; }

        [Display(Name = "Engine Displacement")]
        [Range(50, 7000)]
        public int? EngineDisplacement { get; set; }

        [Display(Name = "ALS (Antilag System)")]
        public bool IsAntilagMounted { get; set; }

        [Display(Name = "Launch Control")]
        public bool IsLaunchControlMounted { get; set; }

        [Display(Name = "N2O System")]
        public bool IsN2oMounted { get; set; }
    }
}