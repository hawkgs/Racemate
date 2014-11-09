namespace Racemate.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Racemate.Data.Common.Models;

    public class Car : AuditInfo, IDeletableEntity
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

        public bool IsN2oMounted { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
