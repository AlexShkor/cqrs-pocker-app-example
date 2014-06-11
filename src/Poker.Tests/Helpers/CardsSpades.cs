using System.Collections.Generic;
using Poker.Domain.Data;

namespace Poker.Tests
{
    public static partial class Cards
    {
        public static List<Card> SpadesTwo(this List<Card> cards)
        {
            return cards.Two(Suit.Spades);
        }

        public static List<Card> SpadesThree(this List<Card> cards)
        {
            return cards.Three(Suit.Spades);
        }

        public static List<Card> SpadesFour(this List<Card> cards)
        {
            return cards.Four(Suit.Spades);
        }

        public static List<Card> SpadesFive(this List<Card> cards)
        {
            return cards.Five(Suit.Spades);
        }

        public static List<Card> SpadesSix(this List<Card> cards)
        {
            return cards.Six(Suit.Spades);
        }

        public static List<Card> SpadesSeven(this List<Card> cards)
        {
            return cards.Seven(Suit.Spades);
        }

        public static List<Card> SpadesEight(this List<Card> cards)
        {
            return cards.Eight(Suit.Spades);
        }

        public static List<Card> SpadesNine(this List<Card> cards)
        {
            return cards.Nine(Suit.Spades);
        }

        public static List<Card> SpadesTen(this List<Card> cards)
        {
            return cards.Ten(Suit.Spades);
        }

        public static List<Card> SpadesJack(this List<Card> cards)
        {
            return cards.Jack(Suit.Spades);
        }

        public static List<Card> SpadesQueen(this List<Card> cards)
        {
            return cards.Queen(Suit.Spades);
        }

        public static List<Card> SpadesKing(this List<Card> cards)
        {
            return cards.King(Suit.Spades);
        }

        public static List<Card> SpadesAce(this List<Card> cards)
        {
            return cards.Ace(Suit.Spades);
        }
    }
}