using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Racemate.Web.Startup))]
namespace Racemate.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
