using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AKQ.Domain.Documents;
using AKQ.Domain.Utils;

namespace AKQ.Domain.PBN
{
    public static class BridgeGameExt
    {
        public static string GetPBNForRoboBridge(this BridgeGame game)
        {
            var writer = new StringWriter();
            writer.WriteLine(String.Empty);
            writer.WriteLine("[Dealer \"{0}\"]", game.PlayersManager.FirstPlayer);
            //            writer.WriteLine("[Vulnerable \"All\"]");
            writer.WriteLine("[Deal \"{0}\"]", game.GetDeal());
            writer.WriteLine("[Auction \"{0}\"]", game.PlayersManager.FirstPlayer);
            game.PBNAuction().ForEach(bid => writer.WriteLine(string.Join(" ", bid)));
            if (!game.IsBidding)
            {
                writer.WriteLine("[Play \"-\"]");
                game.CompletedTricks.ForEach(trick => writer.WriteLine(string.Join(" ", trick.PlayedCards.Select(c => c.Value.ToPBN()))));
                writer.WriteLine(string.Join(" ", game.CurrentTrick.PlayedCards.Select(c => c.Value.ToPBN())));

                //                WriteTricks(writer, FirstPlayer);
                writer.WriteLine(String.Empty);
            }
            return writer.ToString();
        }

        public static string GetPBN(this BridgeGame game)
        {
            var playerOrder = new List<PlayerPosition> { PlayerPosition.West, PlayerPosition.North, PlayerPosition.East, PlayerPosition.South };
            var writer = new StringWriter();
            writer.WriteLine(String.Empty);
            writer.WriteLine("[Dealer \"{0}\"]", game.PlayersManager.FirstPlayer.PlayerPosition.ShortName);
            //            writer.WriteLine("[Vulnerable \"All\"]");
            writer.WriteLine("[Deal \"{0}\"]", GetDeal(game));
            writer.WriteLine("[Auction \"{0}\"]", game.PlayersManager.FirstPlayer);
            PBNAuction(game).ForEach(bid => writer.WriteLine(string.Join(" ", bid)));
            if (!game.IsBidding)
            {
                writer.WriteLine("[Declarer \"{0}\"]", game.Contract.Declarer.ShortName);
                writer.WriteLine("[Contract \"{0}\"]", game.Contract);
                writer.WriteLine("[Result \"{0}\"]", game.TricksWonCount);
                writer.WriteLine("[Play \"{0}\"]", game.Contract.Declarer.GetNextPlayerPosition().ToShortName());
                foreach (var trick in game.CompletedTricks)
                {
                    writer.WriteLine(string.Join(" ", trick.PlayedCards.OrderBy(x => playerOrder.IndexOf(x.Key)).Select(c => c.Value.ToPBN())));
                }
                writer.WriteLine(string.Join(" ", game.CurrentTrick.PlayedCards.Select(c => c.Value.ToPBN())));
                writer.WriteLine(String.Empty);
            }
            return writer.ToString();
        }

        public static string GetPBN(this BridgeDeal deal)
        {
            var writer = new StringWriter();
            writer.WriteLine(String.Empty);
            var dealer = PlayerPosition.FromShortName(deal.BestContract.Position);
            writer.WriteLine("[Deal \"{0}\"]", deal.PBNHand);
            writer.WriteLine("[Declarer \"{0}\"]", dealer.ShortName);
            writer.WriteLine("[Contract \"{0}\"]", deal.BestContract.GetValueAndSuit());
            writer.WriteLine("[Play \"{0}\"]", dealer.GetNextPlayerPosition().ShortName);
            return writer.ToString();
        }

        private static string GetDeal(this BridgeGame game)
        {
            var playerOrder = new List<PlayerPosition> { PlayerPosition.South, PlayerPosition.West, PlayerPosition.North, PlayerPosition.East };
            var suitOrder = new List<Suit> { Suit.Spades, Suit.Hearts, Suit.Diamonds, Suit.Clubs };
            var deal = new StringBuilder();
            deal.Append("S:");
            foreach (var playerPosition in playerOrder)
            {
                var hand = game.OriginalHands[playerPosition];
                foreach (var suit in suitOrder)
                {
                    var cards = hand.GetCards(suit).OrderByDescending(x => x.Rank);
                    foreach (var card in cards)
                    {
                        deal.Append(card.Rank.ToPBN());
                    }
                    if (suit != suitOrder.Last())
                    {
                        deal.Append(".");
                    }
                }
                if (playerPosition != playerOrder.Last())
                {
                    deal.Append(" ");
                }
            }
            return deal.ToString();
        }

        private static List<List<string>> PBNAuction(this BridgeGame game)
        {
            var bids = game.Bids.ToList();
            return bids.Select(x => x.Bid.ToString()).Chunk(4).Select(x => x.ToList()).ToList();
        }


    }
}