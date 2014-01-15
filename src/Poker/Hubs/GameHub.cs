using Microsoft.AspNet.SignalR;

namespace Poker.Hubs
{
    public class GameHub: Hub
    {
        public static IHubContext CurrentContext
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<GameHub>();
            }
        }

        public void Connect(string id)
        {
            Groups.Add(Context.ConnectionId, id);
        }
    }
}