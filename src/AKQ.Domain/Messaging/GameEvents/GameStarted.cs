using AKQ.Domain.Services;

namespace AKQ.Domain.UserEvents
{
    public class GameStarted : GameEvent
    {
        public Contract Contract { get; set; }

        public TournamentInfo TournamentInfo { get; set; }

        public string HostId { get; set; }
    }
}