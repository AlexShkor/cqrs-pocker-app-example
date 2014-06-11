using System.Collections.Generic;
using Poker.Domain.Data;

namespace Poker.Tests
{
    public static partial class Cards
    {
        public static List<Card> HeartsTwo(this List<Card> cards)
        {
            return cards.Two(Suit.Hearts);
        }

        public static List<Card> HeartsThree(this List<Card> cards)
        {
            return cards.Three(Suit.Hearts);
        }

        public static List<Card> HeartsFour(this List<Card> cards)
        {
            return cards.Four(Suit.Hearts);
        }

        public static List<Card> HeartsFive(this List<Card> cards)
        {
            return cards.Five(Suit.Hearts);
        }

        public static List<Card> HeartsSix(this List<Card> cards)
        {
            return cards.Six(Suit.Hearts);
        }

        public static List<Card> HeartsSeven(this List<Card> cards)
        {
            return cards.Seven(Suit.Hearts);
        }

        public static List<Card> HeartsEight(this List<Card> cards)
        {
            return cards.Eight(Suit.Hearts);
        }

        public static List<Card> HeartsNine(this List<Card> cards)
        {
            return cards.Nine(Suit.Hearts);
        }

        public static List<Card> HeartsTen(this List<Card> cards)
        {
            return cards.Ten(Suit.Hearts);
        }

        public static List<Card> HeartsJack(this List<Card> cards)
        {
            return cards.Jack(Suit.Hearts);
        }

        public static List<Card> HeartsQueen(this List<Card> cards)
        {
            return cards.Queen(Suit.Hearts);
        }

        public static List<Card> HeartsKing(this List<Card> cards)
        {
            return cards.King(Suit.Hearts);
        }

        public static List<Card> HeartsAce(this List<Card> cards)
        {
            return cards.Ace(Suit.Hearts);
        }
    }
}