using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(Poker.Web.App_Start.Startup))]
namespace Poker.Web.App_Start
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ContainerConfig.Configure(app);
        }
    }
}