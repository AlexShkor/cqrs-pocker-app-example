using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Domain.Aggregates.Game
{
    public class BiddingInfo
    {
        private int _playersCount;
        public List<BiddingStage> BiddingStages { get; private set; }

        public BiddingStage CurrentStage
        {
            get { return BiddingStages.Last(); }
        }

        public int Stage
        {
            get { return BiddingStages.Count - 1; }
        }

        public BiddingInfo(int playersCount)
        {
            _playersCount = playersCount;
            BiddingStages = new List<BiddingStage>();
            NextStage();
        }

        public void AddBid(BidInfo bid)
        {
            if (bid.BiddingStage != Stage)
            {
                throw new InvalidOperationException("Invalid bidding stage.");
            }
            CurrentStage.Bids[bid.Position] = bid;
        }

        public long GetBank()
        {
            return CurrentStage.GetBank();
        }

        public void NextStage()
        {
            if (BiddingStages.Any() && !CurrentStage.IsFinished())
            {
                throw new InvalidOperationException("Can't got to next bidding stage, untill current is finished");   
            }
            var newStage = new BiddingStage(_playersCount);
            if (BiddingStages.Any())
            {
                var completeBids = CurrentStage.Bids.Where(x => x.Value.IsAllIn() || x.Value.IsFold());
                foreach (var bid in completeBids)
                {
                    newStage.Bids.Add(bid.Key, bid.Value);
                }
            }
            BiddingStages.Add(newStage);

        }

        public bool BigBlindWasLastIfPreFlop()
        {
            return
                Stage != (int) BiddingStagesEnum.PreFlop ||
                CurrentStage.Bids.Values.All(x => x.BidType != BidTypeEnum.BigBlind);
        }
    }
}