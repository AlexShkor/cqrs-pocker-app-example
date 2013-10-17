using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Poker.Platform.Mvc;

namespace Poker.Web.Controllers
{
    [RoutePrefix("home")]
    public class HomeController : BaseController
    {
        [GET("")]
        [GET("", IgnoreRoutePrefix = true)]
        public ActionResult Index()
        {
            return View("Templates/Empty","_BaseLayout");
        }

        [GET("view")]
        public ActionResult ViewTemplateActionResult()
        {
            return PartialView();
        }

        [GET("pages/404", IgnoreRoutePrefix = true)]
        public ActionResult Page404()
        {
            return PartialView();
        }
    }
}