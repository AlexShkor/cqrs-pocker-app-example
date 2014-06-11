using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Data;

namespace Poker.Tests
{
    public static partial class Cards
    {
        public static List<Card> New()
        {
            return new List<Card>();
        }

        #region Ranks

        public static List<Card> Two(this List<Card> cards, params Suit[] suits)
        {
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, Rank.Two));
            }
            return cards;
        }

        public static List<Card> Three(this List<Card> cards, params Suit[] suits)
        {
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, Rank.Three));
            }
            return cards;
        }

        public static List<Card> Four(this List<Card> cards, params Suit[] suits)
        {
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, Rank.Four));
            }
            return cards;
        }

        public static List<Card> Five(this List<Card> cards, params Suit[] suits)
        {
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, Rank.Five));
            }
            return cards;
        }

        public static List<Card> Six(this List<Card> cards, params Suit[] suits)
        {
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, Rank.Six));
            }
            return cards;
        }

        public static List<Card> Seven(this List<Card> cards, params Suit[] suits)
        {
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, Rank.Seven));
            }
            return cards;
        }

        public static List<Card> Eight(this List<Card> cards, params Suit[] suits)
        {
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, Rank.Eight));
            }
            return cards;
        }

        public static List<Card> Nine(this List<Card> cards, params Suit[] suits)
        {
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, Rank.Nine));
            }
            return cards;
        }

        public static List<Card> Ten(this List<Card> cards, params Suit[] suits)
        {
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, Rank.Ten));
            }
            return cards;
        }

        public static List<Card> Jack(this List<Card> cards, params Suit[] suits)
        {
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, Rank.Jack));
            }
            return cards;
        }

        public static List<Card> Queen(this List<Card> cards, params Suit[] suits)
        {
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, Rank.Queen));
            }
            return cards;
        }

        public static List<Card> King(this List<Card> cards, params Suit[] suits)
        {
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, Rank.King));
            }
            return cards;
        }

        public static List<Card> Ace(this List<Card> cards, params Suit[] suits)
        {
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, Rank.Ace));
            }
            return cards;
        }

        #endregion

        #region Hands

        public static List<Card> TwoPairsJacksFives()
        {
            return New()
                .Jack(Suit.Clubs)
                .Jack(Suit.Diamonds)
                .Five(Suit.Spades)
                .Five(Suit.Hearts)
                .Six(Suit.Spades)
                .Four(Suit.Spades)
                .Four(Suit.Hearts);
        } 
        public static List<Card> TwoPairsJacksSixes()
        {
            return New()
                .Jack(Suit.Clubs)
                .Jack(Suit.Diamonds)
                .Five(Suit.Spades)
                .Five(Suit.Hearts)
                .Six(Suit.Spades)
                .Six(Suit.Diamonds)
                .Four(Suit.Hearts);
        } 

        #endregion
    }
}