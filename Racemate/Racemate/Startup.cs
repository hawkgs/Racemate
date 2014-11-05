using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Racemate.Startup))]
namespace Racemate
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
