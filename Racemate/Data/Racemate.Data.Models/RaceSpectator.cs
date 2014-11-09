namespace Racemate.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Racemate.Data.Common.Models;

    public class RaceSpectator : AuditInfo, IDeletableEntity
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

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
