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

        [SetUp]
        public void SetUp()
        {
            _state = new GameTableState();
            var pack = new Pack();
            _cards = pack.GetAllCards();
            _state.Invoke(new TableCreated
            {
                Id = "1",
                BuyIn = 1000,
                MaxPlayers = 10,
                Name = "name",
                SmallBlind = 5
            });
            _state.Invoke(new PlayerJoined
            {
                Id = "1",
                Position = 1,
                UserId = "userId",
                Cash = 100
            });
            _state.Invoke(new GameCreated
            {
                Id = "1",
                GameId = "game1",
                Cards = _cards,
                Players = new List<TablePlayer> { new TablePlayer
                {
                    Cash = 100,
                    Position = 1,
                    UserId = "userId"
                }}
            });
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
            Assert.AreEqual(1,_state.Players.Count);
            Assert.AreEqual("userId", _state.Players[1].UserId);
            Assert.AreEqual(1, _state.Players[1].Position);
        }
    }
}