using System;
 
using AKQ.Domain.Utils;

namespace AKQ.Domain
{
    [Serializable]
    public struct Card
    {
        private readonly Rank _rank;
        private readonly Suit _suit;

        public Card(Suit suit, Rank rank)
        {
            Guard.Against(suit == Suit.NoTrumps, "Cannot create a card without a suit");
            _suit = suit;
            _rank = rank;
        }

        public string CardName
        {
            get { return String.Format("{0}{1}", Suit.ShortName, Value); }
        }

        public string Value { get { return Rank.ShortName; } }

        public Rank Rank
        {
            get { return _rank; }
        }

        public Suit Suit
        {
            get { return _suit; }
        }

        public int ScoreValue
        {
            get { return Rank.Score; }
        }

        public override string ToString()
        {
            return CardName;
        }

        public static Card FromString(string suit, string value)
        {
            return new Card(Suit.FromShortName(suit), Rank.FromString(value));
        }
        public static Card FromString(string cardToPlay)
        {
            Guard.Against<ArgumentException>(cardToPlay.Length > 3, "Cannot create card from string: {0}", cardToPlay);
            return FromString(cardToPlay[0].ToString(), cardToPlay.Substring(1));
        }

        public bool IsSuit(Suit suit)
        {
            return suit == Suit;
        }
    }
}