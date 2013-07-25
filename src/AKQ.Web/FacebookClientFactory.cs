using System;
using System.Web;
using AKQ.Web.Controllers;
using Facebook;

namespace AKQ.Web
{
    public class FacebookClientFactory
    {
        public FacebookClient GetClient()
        {
            var facebookClient = new FacebookClient();
            try
            {
                var token = HttpContext.Current.Session[BaseController.SessionKeys.FbAccessToken] as string;
                facebookClient.AccessToken = token;
            }
            catch
            {

            }
            return facebookClient;
        }
    }
}