using System;
using System.Web.Mvc;
using AKQ.Domain.Documents;
using AKQ.Web.Authentication;
using MongoDB.Bson;
using PAQK.Platform.Domain;
using PAQK.Platform.Domain.Interfaces;
using StructureMap.Attributes;

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

        [SetterProperty]
        public ICommandBus CommandBus { get; set; }

        protected string UserId
        {
            get
            {
                if (User != null)
                {
                    return User.Id;
                }
                return null;
            }
        }

        protected string UserName
        {
            get
            {
                    return User != null ? User.Username : DefaultUsername;
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

        protected void Send(params ICommand[] commands)
        {
            foreach (var command in commands)
            {
                command.Metadata.UserId = UserId;
            }
            CommandBus.Send(commands);
        }
    }
}
