namespace Racemate.Web.Infrastructure.Caching.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Caching;

    public interface ICacheService<T>
        where T : class
    {
        string Key { get; }

        IEnumerable<T> Get(IQueryable<T> collection, int minDuration);
    }
}
