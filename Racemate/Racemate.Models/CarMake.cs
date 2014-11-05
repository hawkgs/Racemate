namespace Racemate.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CarMake
    {
        public CarMake()
        {
            this.CarModels = new HashSet<CarModel>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<CarModel> CarModels { get; set; }
    }
}
