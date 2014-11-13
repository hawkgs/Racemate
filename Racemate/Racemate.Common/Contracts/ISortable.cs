namespace Racemate.Common.Contracts
{
    public interface ISortable
    {
        string SortBy { get; }

        string Order { get; }

        bool IsDescending { get; }
    }
}
