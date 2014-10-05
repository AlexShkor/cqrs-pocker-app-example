using System.Collections.Generic;
using System.Linq;

namespace Poker.Domain.Aggregates.Game
{
    public class BiddingStage
    {
        private readonly int _playersCount;
        public Dictionary<int, BidInfo> Bids { get; private set; }

        public BiddingStage(int playersCount)
        {
            _playersCount = playersCount;
            Bids = new Dictionary<int, BidInfo>();
        }

        public long GetBank()
        {
            return Bids.Values.Select(x => x.Bid).Sum();
        }

        public bool IsFinished()
        {
            var maxBid = Bids.Values.Max(x => x.Bid);
            return Bids.Count == _playersCount && Bids.Values.All(x => x.IsAllIn() || x.IsFold() || x.Bid == maxBid);
        }

        public long GetBetForPlayer(int position)
        {
            return Bids.ContainsKey(position) ? Bids[position].Bet : 0;
        }
    }
}