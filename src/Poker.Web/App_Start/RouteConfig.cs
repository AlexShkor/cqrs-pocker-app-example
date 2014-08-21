using System.Security.Policy;
using System.Web.Mvc;
using System.Web.Routing;

namespace Poker.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               "Default", "{*path}",
                defaults: new { controller = "Home", action = "Index"}
            );
        }
    }
}