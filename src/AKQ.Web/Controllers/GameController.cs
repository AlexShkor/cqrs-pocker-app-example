using System.Web.Mvc;
using AttributeRouting;
using PAQK;
using PAQK.Domain.Aggregates.Game.Commands;
using AttributeRouting.Web.Mvc;
using PAQK.ViewModel;
using PAQK.ViewServices;

namespace AKQ.Web.Controllers
{
    [RoutePrefix("game")]
    [Authorize]
    public class GameController : BaseController
    {
        private readonly TableViewService _tables;

        public GameController(TableViewService tables)
        {
            _tables = tables;
        }

        [GET("")]
        public ActionResult Index(string tableId)
        {
            return PartialView("Game");
        }


        [POST("view/{tableId}")]
        public ActionResult Load(string tableId)
        {
            var table = _tables.GetById(tableId);
            var model = new GameViewModel(table, UserId);
            return Json(model);
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