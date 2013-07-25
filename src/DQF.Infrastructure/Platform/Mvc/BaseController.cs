using System.Web.Mvc;
using PAQK.Common.Account;
using PAQK.Platform.Extensions;
using PAQK.ViewServices;
using StructureMap.Attributes;

namespace PAQK.Platform.Mvc
{
    public abstract class BaseController : Controller
    {
        protected new UserIdentity User { get { return Session[AppConstants.UserIdentitySessionKey] as UserIdentity; } }

        [SetterProperty]
        public SiteSettings Settings { get; set; }

        [SetterProperty]
        public UsersViewService UserViewService { get; set; }

        public virtual T Bind<T>() where T : class, new()
        {
            var model = new T();
            UpdateModel(model);
            return model;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            // restore UserIdentitySessionKey for our custom Auth attribute
            RestoreAuthFromCookies();

            base.OnAuthorization(filterContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SaveSessionDataToViewBag();
            base.OnActionExecuting(filterContext); 
        }

        private void SaveSessionDataToViewBag()
        {
            if (User != null)
            {
                ViewBag.UserName = User.UserName ?? User.Email;
                ViewBag.UserId = User.UserId;
            }

            if (Settings != null)
            {
                ViewBag.Environment = Settings.AppEnvironment;
            }
        }

        private void RestoreAuthFromCookies()
        {
            // this is need to authorize user from .auth cookie  when session ends (when rebuild project for example)
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated && (User == null || !User.UserId.HasValue()))
            {
                var user = UserViewService.GetByEmail(System.Web.HttpContext.Current.User.Identity.Name);
                if (user != null)
                {
                  
                    Session[AppConstants.UserIdentitySessionKey] = new UserIdentity(user);

                }
            }
        }

        public RedirectResult RedirectToRefferer()
        {
            return Redirect(Request.UrlReferrer != null ? Request.UrlReferrer.LocalPath : "/");
        }


        protected string GetRouteValue(string paramName)
        {
            return RouteData.Values[paramName].ToString();
        }

        protected ActionResult PermissionsErrorResult()
        {
            return RedirectToAction("PermissionsError", "Account");
        }

        public new JsonResult Json(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}