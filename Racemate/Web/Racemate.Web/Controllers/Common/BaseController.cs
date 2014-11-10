namespace Racemate.Web.Controllers.Common
{
    using System.Web.Mvc;
    using Racemate.Data;

    public class BaseController : Controller
    {
        protected readonly IRacemateData data;

        public BaseController(IRacemateData data)
        {
            this.data = data;
        }
    }
}