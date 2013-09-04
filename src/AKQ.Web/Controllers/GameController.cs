using System.Web.Mvc;
using AttributeRouting;
using PAQK.Domain.Aggregates.Game.Commands;
using AttributeRouting.Web.Mvc;

namespace AKQ.Web.Controllers
{
    [RoutePrefix("game")]
    public class GameController : BaseController
    {
        [GET("view/{gameId}")]
        public ActionResult Index(string gameId)
        {
            ViewBag.Title = "Game";
            ViewBag.UserName = UserName;
            return View("Templates/Game", (object)gameId);
        }


        [POST("view/{gameId}")]
        public ActionResult Load(string gameId)
        {
            ViewBag.Title = "Game";
            ViewBag.UserName = UserName;
            return View("Templates/Game", (object)gameId);
        }

        [POST("join")]
        public ActionResult Join(string tableId)
        {
            var cmd = new JoinTable
            {
                Id = tableId,
                Cash = 1000,
                UserId = UserId
            };
            Send(cmd);
            return Json(cmd);
        }

        [POST("force")]
        public ActionResult Force(string tableId)
        {
            var cmd = new JoinTable
            {
                Id = tableId,
                Cash = 1000,
                UserId = UserId
            };
            Send(cmd);
            return Json(cmd);
        }
    }
}