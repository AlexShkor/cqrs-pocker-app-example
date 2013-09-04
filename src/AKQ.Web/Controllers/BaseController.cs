using System;
using System.Web.Mvc;
using AKQ.Web.Authentication;
using MongoDB.Bson;
using PAQK.Platform.Domain;
using PAQK.Platform.Domain.Interfaces;
using PAQK.Views;
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
                return Request.IsAuthenticated ? ((AkqIdentity)User.Identity).Email : null;
            }
        }

        protected string UserName
        {
            get
            {
                return Request.IsAuthenticated ? User.Identity.Name : null;
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
                ViewBag.UserEmail = UserId;

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
