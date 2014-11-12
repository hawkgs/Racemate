namespace Racemate.Web.Models.Common
{
    public interface ISortable
    {
        string SortBy { get; }

        string Order { get; }

        bool IsDescending { get; }
    }
}
