using System.Collections.Generic;
using NUnit.Framework;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Tests.AggregateActionsTest.JoinPlayer
{
    public class JoinSecondPlayerSuccessful : AggregateTest<GameTableAggregate, GameTableState>
    {
        public override void Given(GameTableAggregate a)
        {
            a.CreateTable("123", "table", 100, 2);
            a.JoinTable("me1", 100);
        }

        public override void When(GameTableAggregate a)
        {
            a.JoinTable("me2", 100);
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new PlayerJoined
            {
                Id = "123",
                Cash = 100,
                Position = 2,
                UserId = "me2"
            };
            yield return new GameCreated
            {
                Id = "123",
                Players = new List<TablePlayer>()
                {
                    new TablePlayer
                    {
                        UserId = "me1",
                        Position = 1,
                        Cash = 100
                    },
                    new TablePlayer
                    {
                        UserId = "me2",
                        Position = 2,
                        Cash = 100
                    }
                }
            };
            yield return new CardsDealed
            {
                Id = "123"
            };
            yield return new DealerAssigned
            {
                Id = "123",
                Dealer = new PlayerInfo
                {
                    Position = 1,
                    UserId = "me1"
                },
                SmallBlind = new PlayerInfo
                {
                    Position = 2,
                    UserId = "me2"
                },
                BigBlind = new PlayerInfo
                {
                    Position = 1,
                    UserId = "me1"
                }
            };
            yield return new BidMade
            {
                Id = "123",
                Bid = new BidInfo
                {
                    UserId = "me2",
                    Position = 2,
                    Bid = 2,
                    Bet = 2,
                    Amount = 2,
                    BidType = BidTypeEnum.SmallBlind,
                    NewCashValue = 98
                }
            };
            yield return new BidMade
            {
                Id = "123",
                Bid = new BidInfo
                {
                    UserId = "me1",
                    Position = 1,
                    Bid = 4,
                    Bet = 4,
                    Amount = 4,
                    BidType = BidTypeEnum.BigBlind,
                    NewCashValue = 96
                }
            };
            yield return new NextPlayerTurned
            {
                Id = "123",
                Player = new PlayerInfo
                {
                    UserId = "me2",
                    Position = 2
                },
                MinBet = 6
            };
        }

        [Test]
        public override void Test()
        {
            ValidateEvents("GameId","Cards");
        }
    }
}