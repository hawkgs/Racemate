namespace Racemate.Web.Areas.User.ViewModels.Garage
{
    using AutoMapper;
    using System.Linq;
    using System.Collections.Generic;
    using Racemate.Data.Models;
    using Racemate.Web.Infrastructure.Mapping;

    public class CarViewModel : IMapFrom<Car>
    {
        public CarModel Model { get; set; }

        public ICollection<RaceType> RaceTypes { get; set; }

        public int Weight { get; set; }

        public int Hp { get; set; }

        public int Torque { get; set; }

        public EngineAspiration EngineAspiration { get; set; }

        public string Year { get; set; }

        public int? EngineDisplacement { get; set; }

        public bool IsAntilagMounted { get; set; }

        public bool IsLaunchControlMounted { get; set; }

        public bool IsN2oMounted { get; set; }
    }
}