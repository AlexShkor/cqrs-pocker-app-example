using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Domain.Data;

namespace Poker.Tests.AggregateStateTests
{
    [TestFixture]
    public class GameFinishedTest
    {
        private GameTableState _state;

        [SetUp]
        public void SetUp()
        {
            _state = new GameTableState();
            _state.Invoke(new TableCreated
            {
                Id = "123",
                BuyIn = 1000,
                MaxPlayers = 10,
                Name = "name",
                SmallBlind = 5
            });
            _state.Invoke(new PlayerJoined
            {
                Id = "123",
                Position = 1,
                UserId = "me1",
                Cash = 100
            });
            _state.Invoke(new GameCreated
            {
                Id = "123",
                GameId = "game1",
                Cards = new Pack().GetAllCards(),
                Players = new List<TablePlayer> { new TablePlayer
                {
                    Cash = 100,
                    Position = 1,
                    UserId = "me1"
                }}
            });
        }

        [Test]
        public void ResetsFields()
        {
            _state.Invoke(new GameFinished
            {
                Id = "1",
                Winners = Winners.Me1(50)
            });
            Assert.IsNull(_state.GameId);
            Assert.IsNull(_state.CurrentBidding);
            Assert.AreEqual(0,_state.MaxBid);
            
        }

        [Test]
        public void GivesBankToWinner()
        {
            _state.Invoke(new GameFinished
            {
                Id = "1",
                Winners = Winners.Me1(50)
            });
            Assert.AreEqual(100 + 50, _state.JoinedPlayers["me1"].Cash);
        }
    }
}