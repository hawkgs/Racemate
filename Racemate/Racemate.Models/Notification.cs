namespace Racemate.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Notification
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public string Message { get; set; }

        public bool IsSeen { get; set; }
    }
}
