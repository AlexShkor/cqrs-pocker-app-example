using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Domain.Data;
using Poker.Platform.Domain;

namespace Poker.Tests.AggregateStateTests
{
    public class GameCreatedStateWithThreePlayersTests : GameSetUp
    {
        public override void SetUp()
        {
            base.SetUp();

            var player3 = new TablePlayer()
            {
                Position = 3,
                UserId = "me3",
                Cash = 1000
            };

            _state.Invoke(new PlayerJoined
            {
                Id = TestTableId,
                Position = player3.Position,
                UserId = player3.UserId,
                Cash = player3.Cash,
            });

            _state.Invoke(new GameFinished
            {
                Id = TestTableId,
                GameId = "game_1",
                Winners = Winners.Me1(50)
            });

            var pack = new Pack();
            _state.Invoke(new GameCreated
            {
                Id = TestTableId,
                GameId = "game_2",
                Cards = pack.GetAllCards(),
                Players = _state.CopyPlayers()
            });
        }

        [Test]
        public void TablePropertiesAreSetWithThirdPlayer()
        {
            Assert.AreEqual("game_2", _state.GameId);

            Assert.AreEqual("me1", _state.Players[1].UserId);
            Assert.AreEqual(1, _state.Players[1].Position);

            Assert.AreEqual("me2", _state.Players[2].UserId);
            Assert.AreEqual(2, _state.Players[2].Position);

            Assert.AreEqual("me3", _state.Players[3].UserId);
            Assert.AreEqual(3, _state.Players[3].Position);

            Assert.AreEqual(3, _state.Players.Count);
            Assert.AreEqual(3, _state.JoinedPlayers.Count);
        }

        [Test]
        public void CardsAreDealtWithThirdPlayer()
        {
            _state.Invoke(new CardsDealed
            {
                Id = TestTableId,
                GameId = "game_2",
                Cards = TakeCards(_state.CopyPlayers(), _state.Pack)
            });


            var currentCards = _state.Pack.GetAllCards();
            Assert.AreEqual(46, currentCards.Count);
            Assert.AreEqual(2, _state.Players[1].Cards.Count);
            Assert.AreEqual(2, _state.Players[2].Cards.Count);
            Assert.AreEqual(2, _state.Players[3].Cards.Count);

            var playersCards = new List<Card>();
            playersCards.AddRange(_state.Players[1].Cards);
            playersCards.AddRange(_state.Players[2].Cards);
            playersCards.AddRange(_state.Players[3].Cards);

            foreach (var card in playersCards)
            {
                var similar = currentCards.FindAll(c => c.Rank == card.Rank && c.Suit == card.Suit);
                Assert.AreEqual(0, similar.Count());
            }
        }

        [Test]
        public void BlindsAreAssignedtWithThirdPlayer()
        {
            _state.Invoke(new CardsDealed
            {
                Id = TestTableId,
                GameId = "game_2",
                Cards = TakeCards(_state.CopyPlayers(), _state.Pack)
            });

            _state.Invoke(new DealerAssigned
            {
                Id = TestTableId,
                GameId = "game_1",
                Dealer = new PlayerInfo
                {
                    Position = _state.Players[2].Position,
                    UserId = _state.Players[2].UserId
                },
                SmallBlind = new PlayerInfo
                {
                    Position = _state.Players[3].Position,
                    UserId = _state.Players[3].UserId
                },
                BigBlind = new PlayerInfo
                {
                    Position = _state.Players[1].Position,
                    UserId = _state.Players[1].UserId,
                }
            });

            _state.Invoke(new BidMade
            {
                Id = TestTableId,
                Bid = new BidInfo
                {
                    UserId = _state.Players[3].UserId,
                    Position = _state.Players[3].Position,
                    Bid = 5,
                    Odds = 5,
                    NewCashValue = 995,
                    BidType = BidTypeEnum.SmallBlind,
                }
            });

            _state.Invoke(new BidMade
            {
                Id = TestTableId,
                Bid = new BidInfo
                {
                    UserId = _state.Players[1].UserId,
                    Position = _state.Players[1].Position,
                    Bid = 10,
                    Odds = 10,
                    NewCashValue = 1030,
                    BidType = BidTypeEnum.BigBlind,
                }
            });

            Assert.AreEqual(10, _state.Players[1].Bid);
            Assert.AreEqual(0, _state.Players[2].Bid);
            Assert.AreEqual(5, _state.Players[3].Bid);
        }
    }
}