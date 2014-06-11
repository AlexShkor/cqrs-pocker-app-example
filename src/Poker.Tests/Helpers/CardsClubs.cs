using System.Collections.Generic;
using Poker.Domain.Data;

namespace Poker.Tests
{
    public static partial class Cards
    {
        public static List<Card> ClubsTwo(this List<Card> cards)
        {
            return cards.Two(Suit.Clubs);
        }

        public static List<Card> ClubsThree(this List<Card> cards)
        {
            return cards.Three(Suit.Clubs);
        }

        public static List<Card> ClubsFour(this List<Card> cards)
        {
            return cards.Four(Suit.Clubs);
        }

        public static List<Card> ClubsFive(this List<Card> cards)
        {
            return cards.Five(Suit.Clubs);
        }

        public static List<Card> ClubsSix(this List<Card> cards)
        {
            return cards.Six(Suit.Clubs);
        }

        public static List<Card> ClubsSeven(this List<Card> cards)
        {
            return cards.Seven(Suit.Clubs);
        }

        public static List<Card> ClubsEight(this List<Card> cards)
        {
            return cards.Eight(Suit.Clubs);
        }

        public static List<Card> ClubsNine(this List<Card> cards)
        {
            return cards.Nine(Suit.Clubs);
        }

        public static List<Card> ClubsTen(this List<Card> cards)
        {
            return cards.Ten(Suit.Clubs);
        }

        public static List<Card> ClubsJack(this List<Card> cards)
        {
            return cards.Jack(Suit.Clubs);
        }

        public static List<Card> ClubsQueen(this List<Card> cards)
        {
            return cards.Queen(Suit.Clubs);
        }

        public static List<Card> ClubsKing(this List<Card> cards)
        {
            return cards.King(Suit.Clubs);
        }

        public static List<Card> ClubsAce(this List<Card> cards)
        {
            return cards.Ace(Suit.Clubs);
        }
    }
}