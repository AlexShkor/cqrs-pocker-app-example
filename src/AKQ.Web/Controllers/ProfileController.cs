using System.Web.Mvc;
using AKQ.Domain.Services;
using AKQ.Web.Models;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace AKQ.Web.Controllers
{
    [Authorize]
    [RoutePrefix("profile")]
    public class ProfileController : BaseController
    {
        private readonly UsersService _users;
        private readonly BridgeGameDocumentsService _games;

        public ProfileController(UsersService users, BridgeGameDocumentsService games)
        {
            _users = users;
            _games = games;
        }

        [GET("")]
        [GET("assets/templates/profile", IgnoreRoutePrefix = true)]
        public ActionResult Index()
        {
            var current = _users.GetById(UserId);
            var filter = new BridgeGameFilter()
            {
                HostId = UserId,
                Finished = true,
            };
            var totalGames = _games.Count(filter);
            filter.Won = true;
            var totalWons = _games.Count(filter);
            var model = new ProfileViewModel(current, totalGames, totalWons);
            return View(model);
        }

    }
}
