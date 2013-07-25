using AKQ.Domain;

namespace AKQ.Tests
{
    public static class TrickTestExtenssions
    {
        public static Trick With(this Trick trick, PlayerPosition position, Suit suit, Rank rank)
        {
            trick.AddCard(position, new Card(suit, rank));
            return trick;
        }

        public static Trick West(this Trick trick, Suit suit, Rank rank)
        {
            return trick.With(PlayerPosition.West, suit, rank);
        }

        public static Trick North(this Trick trick, Suit suit, Rank rank)
        {
            return trick.With(PlayerPosition.North, suit, rank);
        }

        public static Trick East(this Trick trick, Suit suit, Rank rank)
        {
            return trick.With(PlayerPosition.East, suit, rank);
        }

        public static Trick South(this Trick trick, Suit suit, Rank rank)
        {
            return trick.With(PlayerPosition.South, suit, rank);
        }
    }
}