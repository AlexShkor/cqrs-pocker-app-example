﻿using System;
using System.Collections.Generic;
using System.Linq;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.ApplicationServices.Hands;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices
{
    public class WinnerDetector
    {
        private readonly SortedList<int, PokerHandPrototype> _prototypes = new SortedList<int, PokerHandPrototype>();

        private readonly Dictionary<string, IPokerHand> _users = new Dictionary<string, IPokerHand>();
        private readonly Dictionary<IPokerHand, string> _hands = new Dictionary<IPokerHand, string>();

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
                    _hands[prototype] = userId;
                    keyvalue.Value.ReplaceInstance();
                }
            }
        }

        public IEnumerable<WinnerResult> GetWinners(long bank)
        {
            var groups = _users.GroupBy(x => x.Value.Score).ToList();
            var maxScore = groups.Max(x => x.Key);
            var sameCombination = groups.Single(x => x.Key == maxScore).ToList();
            if (sameCombination.Count > 1)
            {
                var ordered = GetOrdered(_users.Values);
                foreach (var hands in ordered)
                {
                    var count = hands.Count();
                    var prize = bank/count;
                    foreach (var hand in hands)
                    {
                        yield return new WinnerResult
                        {
                            UserId = _hands[hand],
                            PokerHand = hand,
                            Prize = prize
                        };
                    }
                    break;
                }
            }
            else
            {
                yield return sameCombination.Select(x => new WinnerResult()
                {
                    UserId = x.Key,
                    PokerHand = x.Value,
                    Prize = bank
                }).Single();
            }
        }

        public IPokerHand[][] GetOrdered(IEnumerable<IPokerHand> hands)
        {
            var sortedList = new SortedList<int, List<IPokerHand>>();
            var nextOrder = 1;
            var handsOrdered = hands.OrderByDescending(x => x);
            IPokerHand previous = null;
            foreach (var hand in handsOrdered)
            {
                if (previous == null || previous.CompareTo(hand) != 0)
                {
                    sortedList.Add(nextOrder, new List<IPokerHand>());
                    nextOrder++;
                }
                sortedList[nextOrder - 1].Add(hand);
                previous = hand;
            }
            return sortedList.Select(x => x.Value.ToArray()).ToArray();
        } 
    }

    public class WinnerResult
    {
        public string UserId { get; set; }
        public IPokerHand PokerHand { get; set; }
        public long Prize { get; set; }
    }
}