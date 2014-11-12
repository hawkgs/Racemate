namespace Racemate.Common
{
    using System;

    public static class Paging
    {
        public static int GetCurrentPage(int? page)
        {
            int pageParam = 0;

            if (page.HasValue && page > 0)
            {
                pageParam = page.Value - 1;
            }

            return pageParam;
        }

        public static int GetPageCount(int total, int pageSize)
        {
            return (int)Math.Ceiling((double)total / (double)pageSize);
        }
    }
}
