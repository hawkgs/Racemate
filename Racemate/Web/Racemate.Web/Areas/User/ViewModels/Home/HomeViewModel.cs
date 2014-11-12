namespace Racemate.Web.Areas.User.ViewModels.Home
{
    using System.Collections.Generic;

    public class HomeViewModel
    {
        public IEnumerable<RaceThumbViewModel> LatestRaces { get; set; }

        public IEnumerable<UserThumbViewModel> TopMembers { get; set; }
    }
}