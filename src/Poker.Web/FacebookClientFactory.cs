using System.Web;
using Facebook;
using Poker.Platform.Mvc;

namespace Poker.Web
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