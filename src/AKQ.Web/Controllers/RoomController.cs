using System.Linq;
using System.Web.Mvc;
using AKQ.Domain;
using AKQ.Domain.Services;
using AKQ.Domain.ViewModel.Room;
using AKQ.Web.Controllers.ActionResults;
using AKQ.Web.Models;
using RestSharp.Extensions;
using MongoDB.Bson;

namespace AKQ.Web.Controllers
{
    public class RoomController : BaseController
    {
        private readonly ConnectionsViewService _connections;
        private readonly UsersService _users;
        private readonly GamesManager _gamesManager;

        public RoomController(ConnectionsViewService connections, UsersService users, GamesManager gamesManager)
        {
            _connections = connections;
            _users = users;
            _gamesManager = gamesManager;
        }

        public ActionResult Index()
        {
            var gameId = _gamesManager.PopGame();
            if (gameId.HasValue())
            {
                return RedirectToAction("JoinGame", "Game", new {id = gameId});
            }
            return View();
        }

        public ActionResult Find()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonNetResult Load()
        {
            var token = ObjectId.GenerateNewId().ToString();
            _users.SetConnectionToken(UserId,token);
            var model = new RoomViewModel
            {
                UserId = UserId,
                UserName = UserName,
                Token = token,
                Players = _connections.GetAll().Select(x => new RoomPlayerViewModel(x)).ToList()
            };
            return new JsonNetResult(model);
        }

    }
}
