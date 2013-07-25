using System.Collections.Generic;

namespace AKQ.Domain.PokerSets
{
    public class PokerSetsNames
    {
        public const string OnePair = "One Pair";
    }

    public abstract class BasePokerSet: IPokerSet
    {
        public abstract string Name { get; }
        public abstract int Score { get; }
        public abstract bool IsPresent(List<Card> cards);
    }

    public interface IPokerSet
    {
        string Name { get; }

        int Score { get; }

        bool IsPresent(List<Card> cards);
    }

    public class OnePairSet : IPokerSet
    {
        public string Name 
        {
            get { return PokerSetsNames.OnePair; }
        }

        public int Score { get { return (int) PokerSetScores.OnePair; } }

        public bool IsPresent(List<Card> cards)
        {
            throw new System.NotImplementedException();
        }
    }

    public enum PokerSetScores
    {
        HighCard = 0,
        OnePair = 1,
        TwoPair = 2,
        Set = 3,
        Straight = 4,
        Flush = 5,
        FullHouse = 6,
        Quads = 7,
        StraightFlush = 8,
        RoyalFlush = 9,
    }
}
