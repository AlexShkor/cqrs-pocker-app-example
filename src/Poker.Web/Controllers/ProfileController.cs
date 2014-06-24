using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Poker.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AvatarsService _avatars;

        public ProfileController(AvatarsService avatars)
        {
            _avatars = avatars;
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
    }
}
