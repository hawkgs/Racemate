namespace Racemate.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CarModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int CarMakeId { get; set; }

        public virtual CarMake CarMake { get; set; }
    }
}
