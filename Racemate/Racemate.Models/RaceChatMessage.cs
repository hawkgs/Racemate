namespace Racemate.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RaceChatMessage
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public string RaceId { get; set; }

        public Race Race { get; set; }

        public string Message { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
