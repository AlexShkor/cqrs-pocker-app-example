using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace AKQ.Domain.Documents.Progress
{
    public class UserProgress
    {
        [BsonId]
        //the same as user ID
        public string Id { get; set; }

        public RepetitionProgress RepetitionProgress { get; set; }
        public PracticeProgress PracticeProgress { get; set; }

        public UserProgress()
        {
            PracticeProgress = new PracticeProgress();
            RepetitionProgress = new RepetitionProgress();
        }
    }

    public class RepetitionProgress
    {
        public List<DealStats> Deals { get; set; }

        public RepetitionProgress()
        {
            Deals = new List<DealStats>();
        }

        public DealStats GetDeal(string dealId)
        {
            return Deals.Find(x => x.DealId == dealId);
        }

        public DealStats GetCurrentDeal()
        {
            for (int cycle = 1; cycle <= 3; cycle++)
            {
                for (int i = 0; i < Deals.Count; i++)
                {
                    if (Deals[i].DealGameStats.Count < cycle)
                    {
                        return Deals[i];
                    }
                }
            }
            return null;
        }
    }

    public class PracticeProgress
    {
        public List<GameSet> GameSets { get; set; }

        public GameSet GetCurrentGameSet()
        {
            return GameSets.OrderByDescending(x => x.Index).FirstOrDefault(x => !x.IsFinished());
        }

        public PracticeProgress()
        {
            GameSets = new List<GameSet>();
        }

        public int GetLastGameSetIndex()
        {
            return GameSets.Max(x => x.Index);
        }

        public GameSet GetLastGameSet()
        {
            return GameSets.OrderByDescending(x => x.Index).FirstOrDefault();
        }

        public GameSet GetGameSetFor(string dealId)
        {
            return GameSets.Find(x => x.Deals.Any(d => d.DealId == dealId));
        }
    }

    public class GameSet
    {
        public int Index { get; set; }

        public int Skip { get; set; }

        public int DealsCount { get; set; }

        public List<DealStats> Deals { get; set; }

        public GameSet()
        {
            Deals = new List<DealStats>();
        }

        public GameSet(int index, int skip, int count, IEnumerable<BridgeDeal> deals):this()
        {
            Index = index;
            Skip = skip;
            DealsCount = count;
            Deals = deals.Select(x => new DealStats(x)).ToList();
        }

        public bool IsFinished()
        {
            return Deals.All(x => x.HasWin());
        }

        public string GetNextDeal()
        {
            return Deals.First(x => !x.HasWin()).DealId;
        }

        public DealStats GetDeal(string dealId)
        {
            return Deals.Find(x => x.DealId == dealId);
        }
    }

    public class DealStats
    {
        public string DealId { get; set; }
        public ContractDocument Contract { get; set; }
        public int Target { get; set; }
        public string BoardNumber { get; set; }
        public DealTypeEnum DealType { get; set; }

        public List<DealGameStat> DealGameStats { get; set; }

        public DealStats()
        {
            DealGameStats = new List<DealGameStat>();
        }

        public DealStats(BridgeDeal bridgeDeal):this()
        {
            DealId = bridgeDeal.Id;
            DealType = bridgeDeal.DealType;
            BoardNumber = bridgeDeal.BoardNumber;
            Contract = bridgeDeal.BestContract;
            Target = bridgeDeal.BestResult.Tricks;
        }

        public bool HasWin()
        {
            return DealGameStats.Any(g => g.IsWin());
        }

        public bool HasWinWithFirstAttemp()
        {
            return DealGameStats.Any() && DealGameStats.OrderBy(x => x.Finished).First().IsWin();
        }
    }

    public class DealGameStat
    {
        public DealGameStat(string gameId, DateTime finished, int result)
        {
            GameId = gameId;
            Finished = finished;
            Result = result;
        }

        public DealGameStat()
        {
            
        }

        public string GameId { get; set; }

        public int Result { get; set; }

        public DateTime Finished { get; set; }

        public bool IsWin()
        {
            return Result >= 0;
        }
    }
}