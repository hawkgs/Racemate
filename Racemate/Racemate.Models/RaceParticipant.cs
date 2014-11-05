namespace Racemate.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RaceParticipant
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public bool IsKicked { get; set; }

        public int CarId { get; set; }

        public virtual Car Car { get; set; }

        public string RaceId { get; set; }

        public virtual Race Race { get; set; }

        // After race

        public int FinishPosition { get; set; }

        public bool IsFinishPosConfirmed { get; set; }

        public string FinishTime { get; set; }

        // Race evaluation

        public int OrganizationPts { get; set; }

        public int SafetyPts { get; set; }

        // Misc

        public bool IsPoliceAlerted { get; set; }
    }
}
