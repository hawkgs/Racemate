namespace Racemate.Web.Controllers.Common
{
    using System.Linq;
    using System.Web.Mvc;
    using Racemate.Data;
    using Racemate.Data.Models;

    public class BaseController : Controller
    {
        protected const int PAGE_SIZE = 10;

        protected readonly IRacemateData data;

        protected virtual User CurrentUser
        {
            get
            {
                return this.data.Users.All()
                    .FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            }
        }

        public BaseController(IRacemateData data)
        {
            this.data = data;
        }

        #region Helpers

        protected int GetPage(int? page)
        {
            int pageParam = 0;

            if (page.HasValue && page > 0)
            {
                pageParam = page.Value - 1;
            }

            return pageParam;
        }

        #endregion
    }
}