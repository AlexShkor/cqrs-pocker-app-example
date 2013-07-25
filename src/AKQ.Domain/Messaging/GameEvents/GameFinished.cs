using System;
using System.Collections.Generic;
using AKQ.Domain.Services;
using AKQ.Domain.ViewModel;

namespace AKQ.Domain.UserEvents
{
    public class GameFinished : GameEvent
    {
        public Dictionary<PlayerPosition, Hand> OriginalHands { get; set; }

        public string GamePBN { get; set; }

        public int Result { get; set; }

        public string RoboBridgePBN { get; set; }

        public TournamentInfo TournamentInfo { get; set; }

        public DateTime Finished { get; set; }

        public string HostId { get; set; }

        public string DealId { get; set; }
    }
}