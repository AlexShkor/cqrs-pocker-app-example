using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Poker.Domain.Aggregates.User.Commands;
using Poker.Platform.Mvc;
using Poker.ViewServices;

namespace Poker.Web.Controllers
{
    [RoutePrefix("profile")]
    public class ProfileController : BaseController
    {
        private readonly AvatarsService _avatars;
        private readonly UsersViewService _users;

        public ProfileController(AvatarsService avatars, UsersViewService users)
        {
            _avatars = avatars;
            _users = users;
        }

        [GET("view")]
        public ActionResult Index()
        {
            return View();
        }

        [GET("choose-avatar")]
        public ActionResult ChooseAvatar()
        {
            return View();
        }

        [POST("avatars")]
        public ActionResult Avatars()
        {
            return Json(_avatars.GetAll());
        }

        [POST("myavatar")]
        public ActionResult MyAvatar()
        {
            var user = _users.GetById(UserId);
            return Json(new {avatarUrl = AvatarsService.GetUrlById(user.AvatarId)});
        }

        [POST("setavatar")]
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
