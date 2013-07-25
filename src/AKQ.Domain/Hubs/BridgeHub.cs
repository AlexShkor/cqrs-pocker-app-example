using Microsoft.AspNet.SignalR;

namespace AKQ.Domain.Hubs
{
    public class BridgeHub : Hub
    {
        public void Join(string gameId)
        {
            Groups.Add(Context.ConnectionId, gameId);
        }
    }
}