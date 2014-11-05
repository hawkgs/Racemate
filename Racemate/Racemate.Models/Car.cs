namespace Racemate.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Car
    {
        public Car()
        {
            this.RaceTypes = new HashSet<RaceType>();
        }

        [Key]
        public int Id { get; set; }

        // Owner

        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        // General

        public int ModelId { get; set; }

        public virtual CarModel Model { get; set; }

        public virtual ICollection<RaceType> RaceTypes { get; set; }

        // Car specs

        public int Weight { get; set; }

        public int Hp { get; set; }

        public int Torque { get; set; }

        public EngineAspiration EngineAspiration { get; set; }

        [Required]
        public string Year { get; set; }

        public int? EngineDisplacement { get; set; }
        
        // Extras

        public bool IsAntilagMounted { get; set; }

        public bool IsLaunchControlMounted { get; set; }

        public bool IsNosMounted { get; set; }

        // Misc

        public string Description { get; set; }
    }
}
