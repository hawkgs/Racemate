namespace Racemate.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Racemate.Data.Common.Models;
    using System.Collections.Generic;

    public class RaceType : AuditInfo, IDeletableEntity
    {
        public RaceType()
        {
            this.Cars = new HashSet<Car>();
            this.Races = new HashSet<Race>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set;}

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Car> Cars { get; set; }

        public virtual ICollection<Race> Races { get; set; }
    }
}
