namespace Racemate.Common.Contracts
{
    using System.Collections.Generic;

    public interface IPageable<T>
        where T : class
    {
        IEnumerable<T> Collection { get; }

        int PageCount { get; }

        int CurrentPage { get; }
    }
}
