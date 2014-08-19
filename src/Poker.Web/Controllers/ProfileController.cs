using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Poker.Domain.Aggregates.User.Commands;
using Poker.Platform.Mvc;
using Poker.ViewServices;

namespace Poker.Web.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly AvatarsService _avatars;
        private readonly UsersViewService _users;

        public ProfileController(AvatarsService avatars, UsersViewService users)
        {
            _avatars = avatars;
            _users = users;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChooseAvatar()
        {
            return View();
        }

        public ActionResult Avatars()
        {
            return Json(_avatars.GetAll());
        }


        public ActionResult MyAvatar()
        {
            var user = _users.GetById(UserId);
            return Json(new {avatarUrl = AvatarsService.GetUrlById(user.AvatarId)});
        }

        public ActionResult SetAvatar(string avatarId)
        {
            var cmd = new SetProfileAvatar
            {
                Id = UserId,
                AvatarId = avatarId
            };
            Send(cmd);
            return Json(new { avatarId });
        }
    }
}
