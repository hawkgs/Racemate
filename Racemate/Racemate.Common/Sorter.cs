namespace Racemate.Common
{
    using System;
    using System.Linq;
    using Racemate.Common.Contracts;
    using System.Linq.Expressions;
    
    public class Sorter<T> : ISorter<T>
        where T : class
    {
        public Sorter(IQueryable<T> collection)
        {
            this.Collection = collection;
        }

        public IQueryable<T> Collection { get; set; }

        public bool SortBy<TOrderBy>(string order, bool isDescending, Expression<Func<T, TOrderBy>> expression)
        {
            if (order == "desc")
            {
                this.Collection = this.Collection.OrderByDescending(expression);
            }
            else
            {
                this.Collection = this.Collection.OrderBy(expression);
                isDescending = true;
            }

            return isDescending;
        }
    }
}
