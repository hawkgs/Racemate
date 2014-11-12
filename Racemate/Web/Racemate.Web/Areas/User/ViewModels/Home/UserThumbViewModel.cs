namespace Racemate.Web.Areas.User.ViewModels.Home
{
    using Racemate.Web.Infrastructure.Mapping;
    using Racemate.Data.Models;

    public class UserThumbViewModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public int Points { get; set; }

        public int FirstPlaces { get; set; }

        public int SecondPlaces { get; set; }

        public int ThirdPlaces { get; set; }
    }
}