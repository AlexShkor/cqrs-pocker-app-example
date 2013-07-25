using System.Threading.Tasks;
using AKQ.Domain.Services;
using AKQ.Domain.ViewModel.Room;
using Microsoft.AspNet.SignalR;

namespace AKQ.Domain.Hubs
{
    public class RoomHub : Hub
    {
        private readonly ConnectionsViewService _connections;

        public RoomHub(ConnectionsViewService connections)
        {
            _connections = connections;
        }

        public static void AcceptPlayer(string connectionId, string gameId)
        {
            GlobalHost.ConnectionManager.GetHubContext<RoomHub>().Clients.Client(connectionId).playerAccepted(gameId);
            GlobalHost.ConnectionManager.GetHubContext<RoomHub>().Clients.All.playerLeft(connectionId);
        }

        public override Task OnConnected()
        {
            return base.OnConnected().ContinueWith(t =>
            {
                var view = new ConnectionView(Context.ConnectionId);
                Clients.All.playerJoined(new RoomPlayerViewModel(view));
                _connections.Save(view);
            });
        }

        public override Task OnDisconnected()
        {
            return base.OnDisconnected().ContinueWith(t =>
            {
                Clients.All.playerLeft(Context.ConnectionId);
                _connections.RemoveById(Context.ConnectionId);
            }); ;
        }
    }
}