namespace Racemate.Web.Areas.User.ViewModels.Race
{
    using System.Collections.Generic;
    using Racemate.Web.Models.Common;

    public class RaceListViewModel : IPaging<RaceGridDetailsViewModel>, ISortable
    {
        public IEnumerable<RaceGridDetailsViewModel> Collection { get; set; }

        public int PageCount { get; set; }

        public int CurrentPage { get; set; }

        public string SortBy { get; set; }

        public string Order { get; set; }

        public bool IsDescending { get; set; }
    }
}