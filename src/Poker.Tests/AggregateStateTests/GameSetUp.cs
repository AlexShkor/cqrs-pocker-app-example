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
    [TestFixture]
    public abstract class GameSetUp
    {
        protected GameTableState _state;
        protected const string TestTableId = "table1";

        [SetUp]
        public virtual void SetUp()
        {
            _state = new GameTableState();
            var pack = new Pack();

            var player1 = new TablePlayer()
            {
                Position = 1,
                UserId = "me1",
                Cash = 1000
            };

            var player2 = new TablePlayer()
            {
                Position = 2,
                UserId = "me2",
                Cash = 1000
            };

            _state.Invoke(new TableCreated
            {
                Id = TestTableId,
                BuyIn = 1000,
                MaxPlayers = 10,
                Name = "My new table",
                SmallBlind = 5
            });

            _state.Invoke(new PlayerJoined
            {
                Id = TestTableId,
                Position = player1.Position,
                UserId = player1.UserId,
                Cash = player1.Cash,
            });

            _state.Invoke(new PlayerJoined
            {
                Id = TestTableId,
                Position = player2.Position,
                UserId = player2.UserId,
                Cash = player2.Cash,
            });

            _state.Invoke(new GameCreated
            {
                Id = TestTableId,
                GameId = "game_1",
                Cards = pack.GetAllCards(),
                Players = _state.CopyPlayers()
            });

            _state.Invoke(new CardsDealed
            {
                Id = TestTableId,
                GameId = "game_1",
                Cards = TakeCards(_state.CopyPlayers(), _state.Pack)
            });

            _state.Invoke(new DealerAssigned
            {
                Id = TestTableId,
                GameId = "game_1",
                Dealer = new PlayerInfo
                {
                    Position = player1.Position,
                    UserId = player1.UserId
                },
                SmallBlind = new PlayerInfo
                {
                    Position = player2.Position,
                    UserId = player2.UserId
                },
                BigBlind = new PlayerInfo
                {
                    Position = player1.Position,
                    UserId = player1.UserId
                }
            });

            _state.Invoke(new BidMade
            {
                Id = TestTableId,
                Bid = new BidInfo
                {
                    UserId = player2.UserId,
                    Position = player2.Position,
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
                    UserId = player1.UserId,
                    Position = player1.Position,
                    Bid = 10,
                    Odds = 10,
                    NewCashValue = 990,
                    BidType = BidTypeEnum.BigBlind,
                }
            });
        }


        protected List<PlayerCard> TakeCards(IEnumerable<TablePlayer> players, Pack pack)
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
