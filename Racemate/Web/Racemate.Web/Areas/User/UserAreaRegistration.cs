using System.Web.Mvc;

namespace Racemate.Web.Areas.User
{
    public class UserAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "User";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "RaceDetails",
                url: "User/Race/Details/{id}",
                defaults: new
                {
                    controller = "RaceDetails",
                    action = "Details",
                    id = UrlParameter.Optional
                }
            );

            context.MapRoute(
                "User_default",
                "User/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}