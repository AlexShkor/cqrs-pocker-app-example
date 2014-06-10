using System.Collections.Generic;
using Poker.Domain.Data;

namespace Poker.Tests
{
    public static  partial class Cards
    {
        public static List<Card> DiamondsTwo(this List<Card> cards)
        {
            return cards.Two(Suit.Diamonds);
        }

        public static List<Card> DiamondsThree(this List<Card> cards)
        {
            return cards.Three(Suit.Diamonds);
        }

        public static List<Card> DiamondsFour(this List<Card> cards)
        {
            return cards.Four(Suit.Diamonds);
        }

        public static List<Card> DiamondsFive(this List<Card> cards)
        {
            return cards.Five(Suit.Diamonds);
        }

        public static List<Card> DiamondsSix(this List<Card> cards)
        {
            return cards.Six(Suit.Diamonds);
        }

        public static List<Card> DiamondsSeven(this List<Card> cards)
        {
            return cards.Seven(Suit.Diamonds);
        }

        public static List<Card> DiamondsEight(this List<Card> cards)
        {
            return cards.Eight(Suit.Diamonds);
        }

        public static List<Card> DiamondsNine(this List<Card> cards)
        {
            return cards.Nine(Suit.Diamonds);
        }

        public static List<Card> DiamondsTen(this List<Card> cards)
        {
            return cards.Ten(Suit.Diamonds);
        }

        public static List<Card> DiamondsJack(this List<Card> cards)
        {
            return cards.Jack(Suit.Diamonds);
        }

        public static List<Card> DiamondsQueen(this List<Card> cards)
        {
            return cards.Queen(Suit.Diamonds);
        }

        public static List<Card> DiamondsKing(this List<Card> cards)
        {
            return cards.King(Suit.Diamonds);
        }

        public static List<Card> DiamondsAce(this List<Card> cards)
        {
            return cards.Ace(Suit.Diamonds);
        }
    }
}