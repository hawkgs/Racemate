namespace Racemate.Web.Areas.User.ViewModels.Race
{
    using System.Collections.Generic;
    using Racemate.Web.Areas.User.ViewModels.Home;
    using Racemate.Common.Contracts;

    public class RaceListViewModel : IPageable<RaceThumbViewModel>, ISortable
    {
        public IEnumerable<RaceThumbViewModel> Collection { get; set; }

        public int PageCount { get; set; }

        public int CurrentPage { get; set; }

        public string SortBy { get; set; }

        public string Order { get; set; }

        public bool IsDescending { get; set; }
    }
}