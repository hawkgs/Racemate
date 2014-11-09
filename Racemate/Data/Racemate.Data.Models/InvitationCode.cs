namespace Racemate.Data.Models
{
    using Racemate.Data.Common.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class InvitationCode : AuditInfo, IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string Code { get; set; }

        public DateTime IssuedOn { get; set; }

        public string CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
