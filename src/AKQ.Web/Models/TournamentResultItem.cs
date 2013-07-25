using System.Collections.Generic;
using System.Linq;
using AKQ.Domain.Documents;
using AKQ.Domain.Utils;

namespace AKQ.Web.Models
{
    public class TournamentResultItem
    {
        public string Id { get; set; }

        public string GameId { get; set; }

        public string Started { get; set; }

        public int MinutesToPlay { get; set; }

        public int HandsToPlay { get; set; }

        public string IsFinishedInTime { get; set; }

        public string TimeSpent { get; set; }

        public List<string> First3Players { get; set; }


        public TournamentResultItem(TournamentDocument doc, string userId)
        {
            Id = doc.Id;
            GameId = doc.Players.Find(x => x.UserId == userId).Games.First().GameId;
            Started = doc.StartTime.ToRelativeDate();
            MinutesToPlay = doc.MinutesToPlay;
            HandsToPlay = doc.HandsToPlay;
            var player = doc.GetPlayerInfo(userId);
            IsFinishedInTime = (player.IsFinishedInTime ?? false).ToYesNo();
            if (player.TournamentFinished.HasValue)
            {
                TimeSpent = (player.TournamentFinished.Value - doc.StartTime).ToString(@"mm\:ss");
            }
        }
    }
}