namespace Racemate.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Racemate.Data.Common.Models;

    public class CarMake : AuditInfo, IDeletableEntity
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

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
