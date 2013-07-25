using System;
using System.Web.Mvc;
using AKQ.Domain.Documents;
using AKQ.Web.Authentication;
using MongoDB.Bson;

namespace AKQ.Web.Controllers
{
    public abstract class BaseController : AsyncController
    {
        private const string DefaultUsername = "Guest";

        public static class SessionKeys
        {
            public const string UserId = "_UserId";
            public const string UserName = "_UserName";
            public const string FbCsrfToken = "fb_csrf_token";
            public const string FbAccessToken = "fb_access_token";
            public const string FbExpiresIn = "fb_expires_in";
        }

        protected string UserId
        {
            get
            {
                if (User != null)
                {
                    return User.Id;
                }
                var id = (string)Session[SessionKeys.UserId];
                if (string.IsNullOrEmpty(id))
                {
                    id = ObjectId.GenerateNewId().ToString();
                    Session[SessionKeys.UserId] = id;
                }
                return id;
            }
        }

        protected string UserName
        {
            get
            {
                var name = (string)Session[SessionKeys.UserName];
                if (string.IsNullOrEmpty(name))
                {
                    name = User != null ? User.Username : DefaultUsername;
                    Session[SessionKeys.UserName] = name;
                }
                return name;
            }
            set
            {
                Session[SessionKeys.UserName] = value;
            }
        }

        protected new User User
        {
            get
            {
                var akqPrincipal = base.User as AkqPrincipal;
                return akqPrincipal != null ? ((AkqIdentity)akqPrincipal.Identity).User : null;
            }
        }

        protected string GenerateId()
        {
            return ObjectId.GenerateNewId().ToString();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var action = (string)RouteData.Values["action"];
            var controller = (string)RouteData.Values["controller"];
            ViewBag.Title = action.Equals("index",StringComparison.InvariantCultureIgnoreCase) ? controller : action;
            if (User != null)
            {
                ViewBag.UserEmail = User.Email;
                ViewBag.UserCreated = User.Registred.ToFileTimeUtc();
            }
            else
            {
                ViewBag.UserEmail = "Guest";
                ViewBag.UserCreated = Session.LCID;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
