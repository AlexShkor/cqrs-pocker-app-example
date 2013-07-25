using System;
using System.Collections.Generic;

namespace AKQ.Domain.Documents
{
    public class TournamentPlayerInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public int NextDealIndex { get; set; }

        public List<TournamentGameInfo> Games { get; set; }

        public bool AllGamesStarted { get; set; }

        public bool AllGamesFinished { get; set; }

        public bool? IsFinishedInTime { get; set; }

        public DateTime? TournamentFinished { get; set; }

        public TournamentPlayerInfo(TournamentGameInfo gameInfo)
        {
            UserId = gameInfo.UserId;
            UserName = gameInfo.UserName;
            NextDealIndex = 1;
            Games = new List<TournamentGameInfo>() { gameInfo };
        }

        public TournamentPlayerInfo()
        {
            
        }
    }
}