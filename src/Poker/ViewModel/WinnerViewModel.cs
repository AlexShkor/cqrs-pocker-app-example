using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.ApplicationServices;

namespace Poker.ViewModel
{
    public class WinnerViewModel
    {
        public string UserId { get; set; }

        public int Position { get; set; }

        public long Amount { get; set; }

        public int HandScore { get; set; }

        public string Hand { get; set; }

        public WinnerViewModel(WinnerInfo winnerInfo)
        {
            UserId = winnerInfo.UserId;
            Position = winnerInfo.Position;
            Amount = winnerInfo.Amount;
            HandScore = winnerInfo.HandScore;
            Hand = ((PokerScores) HandScore).ToString();
        }
    }
}