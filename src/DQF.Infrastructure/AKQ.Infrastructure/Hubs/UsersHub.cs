using Microsoft.AspNet.SignalR;
using PAQK.Authentication;

namespace PAQK.Hubs
{
    public class UsersHub: Hub
    {
        public static IHubContext CurrentContext
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<UsersHub>();
            }
        }

        public void Connect()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                Groups.Add(Context.ConnectionId, ((AkqIdentity)Context.User.Identity).Name);
            }
        }
    }
}