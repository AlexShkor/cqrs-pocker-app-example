using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Poker.Platform.Mvc;

namespace Poker.Web.Controllers
{
    [RoutePrefix("home")]
    [Authorize]
    public class HomeController : BaseController
    {
        [GET("")]
        [GET("", IgnoreRoutePrefix = true)]
        public ActionResult Index()
        {
            return View("Templates/Empty","_BaseLayout");
        }
    }
}