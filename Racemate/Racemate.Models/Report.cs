namespace Racemate.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Report
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public string ReporterId { get; set; }

        public virtual User Reporter { get; set; }

        public ReportType ReportType { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime IssuedOn { get; set; }
    }
}
