namespace Racemate.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RaceRoutePoint
    {
        [Key]
        public int Id { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string RaceId { get; set; }

        public virtual Race Race { get; set; }
    }
}
