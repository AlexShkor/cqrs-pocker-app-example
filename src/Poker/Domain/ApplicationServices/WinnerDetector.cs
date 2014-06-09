using System;
using System.Collections.Generic;
using System.Linq;
using Poker.Domain.ApplicationServices.Hands;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices
{
    public class WinnerDetector
    {
        private readonly SortedList<int, PokerHandPrototype> _prototypes = new SortedList<int, PokerHandPrototype>();

        private readonly Dictionary<string, IPokerHand> _users = new Dictionary<string, IPokerHand>();

        public WinnerDetector()
        {
            AddProrotype<HighCard>();
            AddProrotype<OnePair>();
            AddProrotype<TwoPairs>();
            AddProrotype<Set>();
            AddProrotype<Straight>();
            AddProrotype<Flush>();
            AddProrotype<FullHouse>();
            AddProrotype<Quads>();
            AddProrotype<StraightFlush>();
            AddProrotype<RoyalFlush>();
        }

        private void AddProrotype<T>() where T : IPokerHand, new()
        {
            Func<IPokerHand> creator = () => new T();
            var instance = creator();
            _prototypes.Add(instance.Score, new PokerHandPrototype(instance, creator));
        }

        public class PokerHandPrototype
        {
            public Func<IPokerHand> Creator { get; set; }
            public IPokerHand Instance { get; set; }

            public PokerHandPrototype(IPokerHand instance, Func<IPokerHand> creator)
            {
                Instance = instance;
                Creator = creator;
            }

            public void ReplaceInstance()
            {
                Instance = Creator();
            }
        }

        public void AddPlayer(string userId, List<Card> cards)
        {
            foreach (var keyvalue in _prototypes)
            {
                var prototype = keyvalue.Value.Instance;
                prototype.SetCards(cards);
                if (prototype.IsPresent())
                {
                    _users[userId] = prototype;
                    keyvalue.Value.ReplaceInstance();
                }
            }
        }

        public IEnumerable<WinnerResult> GetWinners()
        {
            var groups = _users.GroupBy(x => x.Value.Score).ToList();
            var maxScore = groups.Max(x => x.Key);
            var sameCombination = groups.Single(x => x.Key == maxScore).ToList();
            if (sameCombination.Count > 1)
            {
                var max = sameCombination.Max(x => x.Value);

                foreach (var keyValuePair in sameCombination)
                {
                    if (keyValuePair.Value.CompareTo(max) == 0)
                    {
                        yield return new WinnerResult
                        {
                            UserId = keyValuePair.Key,
                            PokerHand = keyValuePair.Value
                        };
                    }
                }
            }

            else
            {
                yield return sameCombination.Select(x => new WinnerResult()
                {
                    UserId = x.Key,
                    PokerHand = x.Value
                }).Single();
            }
        }
    }

    public class WinnerResult
    {
        public string UserId { get; set; }

        public IPokerHand PokerHand { get; set; }
    }
}