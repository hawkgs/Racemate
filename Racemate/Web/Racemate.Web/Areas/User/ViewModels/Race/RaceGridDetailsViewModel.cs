namespace Racemate.Web.Areas.User.ViewModels.Race
{
    using Racemate.Web.Infrastructure.Mapping;
    using Racemate.Data.Models;

    public class RaceGridDetailsViewModel : IMapFrom<Race>
    {
        public string Name { get; set; }
    }
}