namespace Racemate.Data.Models
{
    using Racemate.Data.Common.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class RaceChatMessage : AuditInfo, IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public string RaceId { get; set; }

        public Race Race { get; set; }

        [StringLength(150)]
        public string Message { get; set; }

        public DateTime AddedOn { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
