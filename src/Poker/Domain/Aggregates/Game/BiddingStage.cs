using System.Collections.Generic;
using System.Linq;

namespace Poker.Domain.Aggregates.Game
{
    public class BiddingStage
    {
        private readonly int _playersCount;
        public Dictionary<int, BidInfo> Bids { get; private set; }

        public int PlayersCount
        {
            get { return _playersCount; }
        }

        public BiddingStage(int playersCount)
        {
            _playersCount = playersCount;
            Bids = new Dictionary<int, BidInfo>();
        }

        public long GetBank()
        {
            return Bids.Values.Select(x => x.Bet).Sum();
        }

        public bool IsFinished()
        {
            var maxBet = GetMaxBet();
            return Bids.Count == _playersCount && Bids.Values.All(x => x.IsAllIn() || x.IsFold() || x.Bet == maxBet);
        }

        public long GetBetForPlayer(int position)
        {
            return Bids.ContainsKey(position) ? Bids[position].Bet : 0;
        }

        public long GetMaxBet()
        {
            if (!Bids.Any())
            {
                return 0;
            }
            return Bids.Max(x => x.Value.Bet);
        }
    }
}