using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Domain.Data;
using Poker.Platform.Domain;

namespace Poker.Tests.AggregateStateTests
{
    [TestFixture]
    public class GameCreatedStateTests
    {
        private GameTableState _state;
        private List<Card> _cards;

        //[SetUp]
        //public void SetUp()
        //{
        //    _state = new GameTableState();
        //    var pack = new Pack();
        //    _cards = pack.GetAllCards();
        //    _state.Invoke(new TableCreated
        //    {
        //        Id = "1",
        //        BuyIn = 1000,
        //        MaxPlayers = 10,
        //        Name = "name",
        //        SmallBlind = 5
        //    });
        //    _state.Invoke(new PlayerJoined
        //    {
        //        Id = "1",
        //        Position = 1,
        //        UserId = "userId",
        //        Cash = 100
        //    });
        //    _state.Invoke(new GameCreated
        //    {
        //        Id = "1",
        //        GameId = "game1",
        //        Cards = _cards,
        //        Players = new List<TablePlayer> { new TablePlayer
        //        {
        //            Cash = 100,
        //            Position = 1,
        //            UserId = "userId"
        //        }}
        //    });
        //}

        [SetUp]
        public void SetUp()
        {
            _state = new GameTableState();
            var pack = new Pack();
            _cards = pack.GetAllCards();
            var players = new List<TablePlayer>()
            {
                new TablePlayer()
                {
                    Position = 1,
                    UserId = "me1",
                    Cash = 100
                },
                new TablePlayer()
                {
                    Position = 2,
                    UserId = "me2",
                    Cash = 100
                }
            };

            _state.Invoke(new TableCreated
            {
                Id = "table1",
                BuyIn = 1000,
                MaxPlayers = 10,
                Name = "name",
                SmallBlind = 5
            });

            _state.Invoke(new PlayerJoined
            {
                Id = "table1",
                Position = players[0].Position,
                UserId = players[0].UserId,
                Cash = players[0].Cash,
            });

            _state.Invoke(new PlayerJoined
            {
                Id = "table1",
                Position = players[1].Position,
                UserId = players[1].UserId,
                Cash = players[1].Cash,
            });

            _state.Invoke(new GameCreated
            {
                Id = "table1",
                GameId = "game1",
                Cards = _cards,
                Players = players
            });

            _state.Invoke(new CardsDealed
            {
                Id = "table1",
                GameId = "game1",
                Cards = TakeCards(players, pack)
            });

            _state.Invoke(new DealerAssigned
           {
               Id = "table1",
               GameId = "game1",
               Dealer = new PlayerInfo
               {
                   Position = players[0].Position,
                   UserId = players[0].UserId
               },
               SmallBlind = new PlayerInfo
               {
                   Position = players[1].Position,
                   UserId = players[1].UserId
               },
               BigBlind = new PlayerInfo
               {
                   Position = players[0].Position,
                   UserId = players[0].UserId
               }
           });

            _state.Invoke(new BidMade
            {
                Id = "table1",
                Bid = new BidInfo
                {
                    UserId = players[1].UserId,
                    Position = players[1].Position,
                    Bid = 5,
                    Odds = 5,
                    NewCashValue = 995,
                    BidType = BidTypeEnum.SmallBlind,
                }
            });

            _state.Invoke(new BidMade
            {
                Id = "table1",
                Bid = new BidInfo
                {
                    UserId = players[0].UserId,
                    Position = players[0].Position,
                    Bid = 10,
                    Odds = 10,
                    NewCashValue = 990,
                    BidType = BidTypeEnum.BigBlind,
                }
            });
        }


        [Test]
        public void BlindsAreAssigned()
        {
            Assert.AreEqual(10, _state.Players[1].Bid);
            Assert.AreEqual(5, _state.Players[2].Bid);
        }

        [Test]
        public void SetsCards()
        {
            var currentCards = _state.Pack.GetAllCards();
            Assert.AreEqual(_cards.Count, currentCards.Count);
            for (int i = 0; i < _cards.Count; i++)
            {
                Assert.AreEqual(_cards[i], currentCards[i]);
            }
        }

        [Test]
        public void SetsTableProperties()
        {
            Assert.AreEqual("game1", _state.GameId);
            Assert.AreEqual(2, _state.Players.Count);
            Assert.AreEqual("me1", _state.Players[1].UserId);
            Assert.AreEqual(1, _state.Players[1].Position);
        }


        private List<PlayerCard> TakeCards(IEnumerable<TablePlayer> players, Pack pack)
        {
            var takenCards = new List<PlayerCard>();
            foreach (var player in players)
            {
                var cards = pack.TakeFew(2);
                foreach (var card in cards)
                {
                    takenCards.Add(new PlayerCard()
                    {
                        Position = player.Position,
                        UserId = player.UserId,
                        Card = card
                    });
                }
            }

            return takenCards;
        }
    }
}