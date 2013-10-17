using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Poker.Platform.Mvc;

namespace Poker.Web.Controllers
{
    [RoutePrefix("assets/templates")]
    public class TemplatesController : BaseController
    {
        [GET("home")]
        public ActionResult Home()
        {
            return PartialView("Templates/Home");
        }

        [GET("game")]
        public ActionResult Game()
        {
            return PartialView("Templates/Game");
        }

        [GET("replay")]
        public ActionResult Replay()
        {
            return PartialView("Templates/Replay");
        }

        [GET("game-sets")]
        public ActionResult GameSets()
        {
            return PartialView("Templates/GameSets");
        }

        [GET("results/repetition")]
        public ActionResult RepetitionResults()
        {
            return PartialView("Templates/RepetitionResults");
        }

        [GET("history")]
        public ActionResult History()
        {
            return PartialView("Templates/History");
        }
    }
}
