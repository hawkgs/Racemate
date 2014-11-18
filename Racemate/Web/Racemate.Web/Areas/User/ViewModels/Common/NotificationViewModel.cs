namespace Racemate.Web.Areas.User.ViewModels.Common
{
    using Racemate.Data.Models;
    using Racemate.Web.Infrastructure.Mapping;

    public class NotificationViewModel : IMapFrom<Notification>
    {
        public string Message { get; set; }

        public bool IsSeen { get; set; }
    }
}