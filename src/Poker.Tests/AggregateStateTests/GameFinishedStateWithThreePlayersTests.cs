using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Domain.Data;

namespace Poker.Tests.AggregateStateTests
{
    public class GameFinishedStateWithThreePlayersTests : GameSetUp
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

            _state.Invoke(new CardsDealed
            {
                Id = TestTableId,
                GameId = "game_2",
                Cards = TakeCards(_state.CopyPlayers(), _state.Pack)
            });

            _state.Invoke(new DealerAssigned
            {
                Id = TestTableId,
                GameId = "game_2",
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
                    Amount = 5,
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
                    Amount = 10,
                    NewCashValue = 1030,
                    BidType = BidTypeEnum.BigBlind,
                }
            });

            _state.Invoke(new NextPlayerTurned
            {
                Id = TestTableId,
                GameId = "game_2",
                Player = _state.GetPlayerInfo(2)
            });

        }

        [Test]
        public void ResetsFieldsWithThirdPlayer()
        {
            _state.Invoke(new BidMade
            {
                Id = TestTableId,
                Bid = new BidInfo
                {
                    UserId = _state.Players[2].UserId,
                    Position = _state.Players[2].Position,
                    Bid = 60,
                    Amount = 60,
                    NewCashValue = 900,
                    BidType = BidTypeEnum.Raise,
                }
            });

            _state.Invoke(new GameFinished
            {
                Id = TestTableId,
                GameId = "game_2",
                Winners = Winners.Me1(50)
            });

            Assert.IsNull(_state.GameId);
            Assert.IsNull(_state.CurrentBidding);
            Assert.AreEqual(0, _state.MaxBid);
        }




    }
}
