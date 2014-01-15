using Microsoft.AspNet.SignalR;
using Poker.Authentication;

namespace Poker.Hubs
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
                Groups.Add(Context.ConnectionId, ((AkqIdentity)Context.User.Identity).Id);
            }
        }

        public void ConnectToGame(string id)
        {
            Groups.Add(Context.ConnectionId, id);
        }
    }
}