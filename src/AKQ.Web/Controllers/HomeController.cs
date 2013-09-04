using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace AKQ.Web.Controllers
{
    [RoutePrefix("home")]
    public class HomeController : BaseController
    {
        [GET("")]
        public ActionResult Index()
        {
            return View("Templates/Empty","_BaseLayout");
        }
    }
}