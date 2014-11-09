namespace Racemate.Data.Models
{
    using System;
    using Racemate.Data.Common.Models;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class RaceRoutePoint : AuditInfo, IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string RaceId { get; set; }

        public virtual Race Race { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
