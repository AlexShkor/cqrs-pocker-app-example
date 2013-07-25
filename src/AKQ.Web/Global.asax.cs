using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using AKQ.Domain.Documents;
using AKQ.Web.App_Start;
using AKQ.Web.Authentication;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using NLog;
using Segmentio;

namespace AKQ.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            ContainerConfig.Configure();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Analytics.Initialize("kf9yt4pnlb4qf60cvbl1");
        }




        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || authCookie.Value == "")
                return;
            
            FormsAuthenticationTicket authTicket;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch
            {
                return;
            }
            string[] data = authTicket.UserData.Split('|');

            var user = new User {Role = data[0], Username = data[1], Id = authTicket.Name};
            Context.User = new AkqPrincipal(new AkqIdentity(user));
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            if (Request.Url.Host.Equals("akqbridge.apphb.com",StringComparison.InvariantCultureIgnoreCase))
            {
                Response.RedirectPermanent("http://akqbridge.com");
            }
        }
    }
}