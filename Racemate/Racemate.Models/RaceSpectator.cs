namespace Racemate.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RaceSpectator
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public string RaceId { get; set; }

        public virtual Race Race { get; set; }

        // Race evaluation

        public int OrganizationPts { get; set; }

        public int EntertainmentPts { get; set; }

        // Misc

        public bool IsPoliceAlerted { get; set; }
    }
}
