namespace Racemate.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Racemate.Data.Common.Models;

    public class Report : AuditInfo, IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public string ReporterId { get; set; }

        public virtual User Reporter { get; set; }

        public ReportType ReportType { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public DateTime IssuedOn { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
