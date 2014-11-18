namespace Racemate.Web.Infrastructure.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;
    using Racemate.Web.Infrastructure.Caching.Contracts;

    public class CacheService<T> : ICacheService<T>
        where T : class
    {
        public CacheService(string key)
        {
            this.Key = key;
        }

        public string Key { get; private set; }

        public IEnumerable<T> Get(IQueryable<T> collection, int minDuration)
        {
            if (HttpContext.Current.Cache[this.Key] == null)
            {
                HttpContext.Current.Cache.Insert(
                    this.Key,
                    collection.ToList(),
                    null,
                    DateTime.Now.AddMinutes(minDuration),
                    TimeSpan.Zero,
                    CacheItemPriority.Default,
                    null);
            }

            return (IEnumerable<T>)HttpContext.Current.Cache[this.Key];
        }
    }
}
