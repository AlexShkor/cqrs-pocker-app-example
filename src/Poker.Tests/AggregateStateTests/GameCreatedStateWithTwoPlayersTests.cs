using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Poker.Domain.Data;

namespace Poker.Tests.AggregateStateTests
{
    public class GameCreatedStateWithTwoPlayersTests : GameSetUp
    {
        [Test]
        public void TablePropertiesAreSet()
        {
            Assert.AreEqual("game_1", _state.GameId);

            Assert.AreEqual("me1", _state.Players[1].UserId);
            Assert.AreEqual(1, _state.Players[1].Position);

            Assert.AreEqual("me2", _state.Players[2].UserId);
            Assert.AreEqual(2, _state.Players[2].Position);

            Assert.AreEqual(2, _state.Players.Count);
            Assert.AreEqual(2, _state.JoinedPlayers.Count);
        }

        [Test]
        public void CardsAreDealt()
        {
            var currentCards = _state.Pack.GetAllCards();
            Assert.AreEqual(48, currentCards.Count);
            Assert.AreEqual(2, _state.Players[1].Cards.Count);
            Assert.AreEqual(2, _state.Players[2].Cards.Count);

            var playersCards = new List<Card>();
            playersCards.AddRange(_state.Players[1].Cards);
            playersCards.AddRange(_state.Players[2].Cards);

            foreach (var card in playersCards)
            {
                var similarExists = currentCards.Any(c => c.Rank == card.Rank && c.Suit == card.Suit);
                Assert.IsFalse(similarExists);
            }
        }

        [Test]
        public void BlindsAreAssigned()
        {
            Assert.AreEqual(10, _state.Players[1].Bid);
            Assert.AreEqual(5, _state.Players[2].Bid);
        }

    }
}
