namespace Racemate.Web.Areas.User.ViewModels.Race
{
    using System;
    using Racemate.Data.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RaceDataModel
    {
        private const string REQ_ERR_MSG = "{0} field is required!";
        private const string RNG_ERR_MSG = "The {0} must be between {1} and {2}!";
        private const string LEN_ERR_MSG = "The {0} length must be between {2} and {1}!";

        [Display(Name = "Number Of Racers")]
        [Required(ErrorMessage = REQ_ERR_MSG)]
        [Range(2, 25, ErrorMessage = RNG_ERR_MSG)]
        public int AvailableRacePositions { get; set; }

        [Required(ErrorMessage = REQ_ERR_MSG)]
        [Range(2, 25, ErrorMessage = RNG_ERR_MSG)]
        public int Duration { get; set; }

        [Required(ErrorMessage = REQ_ERR_MSG)]
        [StringLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = REQ_ERR_MSG)]
        public float Distance { get; set; }

        [Required(ErrorMessage = REQ_ERR_MSG)]
        [StringLength(30, MinimumLength = 6, ErrorMessage = LEN_ERR_MSG)]
        public string Name { get; set; }

        [Display(Name = "Bet")]
        [Range(1, 100000, ErrorMessage = RNG_ERR_MSG)]
        public int? MoneyBet { get; set; }

        [StringLength(30, MinimumLength = 5, ErrorMessage = LEN_ERR_MSG)]
        public string Password { get; set; }

        [StringLength(500, MinimumLength = 10, ErrorMessage = LEN_ERR_MSG)]
        public string Description { get; set; }

        [Display(Name = "Date and Time")]
        [Required(ErrorMessage = REQ_ERR_MSG)]
        public DateTime DateTimeOfRace { get; set; }

        [Required(ErrorMessage = REQ_ERR_MSG)]
        public ICollection<RaceRoutePoint> Routepoints { get; set; }

        // Unserialized

        [Required(ErrorMessage = REQ_ERR_MSG)]
        public int TypeId { get; set; }
    }
}