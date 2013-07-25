using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AKQ.Domain.Utils;

namespace AKQ.Domain.PBN
{
    public static class Extensions
    {
        public static string ToPBN(this Rank rank)
        {
            return rank.ShortName == "10" ? "T" : rank.ShortName;
        }

        public static string ToPBN(this Trick trick)
        {
            return string.Join(" ", trick.PlayedCards.Select(c => c.Value.ToPBN()));
        }

        public static string ToPBN(this Card card)
        {
            return String.Format("{0}{1}", card.Suit.ShortName, card.Rank.ToPBN());
        }

        static List<Card> GetCards(string pbnHand)
        {
            var list = new List<Card>();
            var suits = pbnHand.Split('.');
            list.AddRange(ReadPbnCard(Suit.Spades, suits[0]));
            list.AddRange(ReadPbnCard(Suit.Hearts, suits[1]));
            list.AddRange(ReadPbnCard(Suit.Diamonds, suits[2]));
            list.AddRange(ReadPbnCard(Suit.Clubs, suits[3]));
            return list;
        }

        static IEnumerable<Card> ReadPbnCard(Suit suit, string cardString)
        {
            return cardString.Aggregate(new List<Card>(), (cards, card) =>
            {
                cards.Add(new Card(suit, Rank.FromChar(card)));
                return cards;
            });
        }

        public static string ToPBN(this Deck playerHands)
        {
            var sb = new StringBuilder();
            sb.Append(playerHands.First().Key);
            foreach (var cards in playerHands.Select(playerHand => playerHand.Value))
            {
                sb.Append(":");
                AppendSuit(cards.GetCards(Suit.Spades), Suit.Spades, sb);
                AppendSuit(cards.GetCards(Suit.Hearts), Suit.Hearts, sb);
                AppendSuit(cards.GetCards(Suit.Diamonds), Suit.Diamonds, sb);
                AppendSuit(cards.GetCards(Suit.Clubs), Suit.Clubs, sb, true);
            }
            return sb.ToString();
        }

        public static void ParseFromPBN(this Deck hands, string pbnHand)
        {
            var pbnhands = pbnHand.Split(':');
            hands.Clear();
            var player = PlayerPosition.FromShortName(pbnhands.First());
            for (var i = 1; i < 5; i++)
            {
                hands.Add(player, new Hand(player, GetCards(pbnhands[i])));
                player = player.GetNextPlayerPosition();
            }
        }

        static void AppendSuit(IEnumerable<Card> cards, Suit suit, StringBuilder sb, bool last = false)
        {
            cards.Where(x => x.Suit == suit)
               .OrderByDescending(x => x.ScoreValue)
               .Each(x => sb.Append(x.Value == "10" ? "T" : x.Value));
            if (!last)
                sb.Append(".");
        }
    }
}