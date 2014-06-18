using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Domain.Data;

namespace Poker.Tests.AggregateStateTests
{
    public class GameFinishedStateWithTwoPlayersTests : GameSetUp
    {
        [Test]
        public void GivesBankToWinner()
        {
            var cash = _state.JoinedPlayers["me1"].Cash;
            _state.Invoke(new GameFinished
            {
                Id = TestTableId,
                Winners = Winners.Me1(50)
            });

            Assert.AreEqual(cash + 50, _state.JoinedPlayers["me1"].Cash);
        }

        [Test]
        public void ResetsFields()
        {
            _state.Invoke(new GameFinished
            {
                Id = TestTableId,
                Winners = Winners.Me1(50)
            });

            Assert.IsNull(_state.GameId);
            Assert.IsNull(_state.CurrentBidding);
            Assert.AreEqual(0, _state.MaxBid);
            
        }
    }
}