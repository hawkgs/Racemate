namespace Racemate.Common.Contracts
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface ISorter<T>
        where T : class
    {
        IQueryable<T> Collection { get; set; }

        bool SortBy<TOrderBy>(string order, bool isDescending, Expression<Func<T, TOrderBy>> expression);
    }
}
