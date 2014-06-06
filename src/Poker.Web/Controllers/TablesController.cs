using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Poker.Domain.Aggregates.Game.Commands;
using Poker.Platform.Mongo;
using Poker.Platform.Mvc;
using Poker.ViewServices;

namespace Poker.Web.Controllers
{
    [Authorize]
    [RoutePrefix("tables")]
    public class TablesController : BaseController
    {
        private readonly TableViewService _tables;
        private readonly IdGenerator _idGenerator;

        public TablesController(TableViewService tables, IdGenerator idGenerator)
        {
            _tables = tables;
            _idGenerator = idGenerator;
        }


        [GET("")]
        public ActionResult Index()
        {
            return PartialView("Tables");
        }

        [POST("load")]
        public ActionResult GetAll()
        {
            var model = _tables.GetAll().OrderBy(x=> x.Players.Count);
            return Json(model);
        }

        [GET("create")]
        public ActionResult New()
        {
            return PartialView("Create");
        }

        [POST("create")]
        public ActionResult Create(string name, long buyIn, long smallBlind)
        {
            var cmd = new CreateTable
            {
                Id = _idGenerator.Generate(),
                BuyIn = buyIn,
                Name = name,
                SmallBlind = smallBlind
            };
            Send(cmd);
            return Json(cmd);
        }

    }
}
