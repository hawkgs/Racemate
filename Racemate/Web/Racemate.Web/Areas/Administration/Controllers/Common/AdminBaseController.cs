namespace Racemate.Web.Areas.Administration.Controllers.Common
{
    using System.Web.Mvc;
    using Racemate.Common;
    using Racemate.Data;
    using Racemate.Web.Controllers.Common;

    [Authorize(Roles = GlobalConstants.ADMIN)]
    public class AdminBaseController : BaseController
    {
        public AdminBaseController(IRacemateData data)
            : base(data)
        {
        }
    }
}