namespace Racemate.Web.Hubs
{
    using Microsoft.AspNet.SignalR;

    public class NotificationsHub : Hub
    {
        public static void NewNotification()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            context.Clients.All.addNewNotification();
        }
    }
}