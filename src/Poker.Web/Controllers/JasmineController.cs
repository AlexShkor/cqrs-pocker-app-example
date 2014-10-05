using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace Poker.Web.Controllers
{
    [RoutePrefix("jasmine")]
    public class JasmineController : Controller
    {
        [GET("")]
        public ActionResult Index()
        {
            return View("JasmineTests");
        }
    }
}
