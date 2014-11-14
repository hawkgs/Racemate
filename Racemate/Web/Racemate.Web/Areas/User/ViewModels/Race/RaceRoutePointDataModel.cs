namespace Racemate.Web.Areas.User.ViewModels.Race
{
    using Racemate.Data.Models;
    using Racemate.Web.Infrastructure.Mapping;

    public class RaceRoutePointDataModel : IMapFrom<RaceRoutePoint>
    {
        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
