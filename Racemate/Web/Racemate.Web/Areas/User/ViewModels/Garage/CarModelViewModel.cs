using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Racemate.Web.Infrastructure.Mapping;
using Racemate.Data.Models;

namespace Racemate.Web.Areas.User.ViewModels.Garage
{
    public class CarModelViewModel : IMapFrom<CarModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}