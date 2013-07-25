using System.Threading.Tasks;
using AKQ.Domain.Services;
using AKQ.Domain.ViewModel.Room;
using Microsoft.AspNet.SignalR;

namespace AKQ.Domain.Hubs
{
    public class TournamentHub : Hub
    {
        private readonly TournamentDocumentsService _tournaments;

        public TournamentHub(TournamentDocumentsService tournaments)
        {
            _tournaments = tournaments;
        }

        public static void Register(string connectionId, string tournamentId)
        {

        }

        public override Task OnConnected()
        {
            return base.OnConnected().ContinueWith(t =>
            {
                
            });
        }

        public override Task OnDisconnected()
        {
            return base.OnDisconnected().ContinueWith(t =>
            {

            }); 
        }
    }
}