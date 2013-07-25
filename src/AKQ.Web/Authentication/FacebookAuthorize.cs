﻿using System.Web.Mvc;
using AKQ.Web.Controllers;

namespace AKQ.Web.Authentication
{
    public class FacebookAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var accessToken = httpContext.Session[BaseController.SessionKeys.FbAccessToken] as string;
            if (string.IsNullOrWhiteSpace(accessToken))
                return false;
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/account/loginwithfacebook");
        }
    }
}