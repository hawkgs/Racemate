namespace Racemate.Web.Controllers.Common
{
    using System.Linq;
    using System.Web.Mvc;
    using Racemate.Data;
    using Racemate.Data.Models;

    public class BaseController : Controller
    {
        protected const int PAGE_SIZE = 3;

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
    }
}