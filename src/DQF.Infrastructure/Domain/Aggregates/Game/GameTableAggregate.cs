using System;
using System.Collections.Generic;
using System.Linq;
using AKQ.Domain;
using AKQ.Domain.UserEvents;
using PAQK.Domain.Aggregates.Game.Data;
using PAQK.Domain.Aggregates.Game.Events;
using PAQK.Domain.Aggregates.Site;
using PAQK.Platform.Domain;
using PAQK.Platform.Domain.Interfaces;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game
{
    public class GameTableAggregate : Aggregate<GameTableState>
    {

        public void CreateTable(string id)
        {
            Apply(new TableCreated()
            {
                Id = id,
            });
        }

        public void CreateGame(string id)
        {
            var pack = new Pack();
            Apply(new GameCreated()
            {
                Id = State.TableId,
                Players = State.JoinedPlayers.Values.ToList(),
                DealerUserId = State.GetNextDealer(),
                Cards = pack.GetAllCards()
            });
            DealCards(id);
        }

        public void FinishGame(string id)
        {
            Apply(new GameFinished()
            {
                Id = id,
            });
        }

        public void JoinTable(string userId, string gameId, int position, long cashInCents)
        {
            if (State.IsTableFull())
            {
                throw new InvalidOperationException("Table is full.");
            }
            if (State.IsPositionTaken(position))
            {
                throw new InvalidOperationException("Position is already taken.");
            }
            Apply(new UserJoined
            {
                GameId = gameId,
                Position = position,
                Id = State.TableId,
                UserId = userId,
                Cash = cashInCents
            });
        }

        private void DealCards(string gameId)
        {
            var takenCards = new List<PlayerCard>();
            for (int i = 0; i < 10; i++)
            {
                var place = State.Places[i];
                if (place.HasUser())
                {
                    var cards = State.Pack.TakeFew(2);
                    foreach (var card in cards)
                    {
                        takenCards.Add(new PlayerCard()
                        {
                            Position = i,
                            UserId = place.UserId,
                            Card = card
                        });
                    }
                }
            }
            Apply(new CardsDealed
            {
                Id = State.TableId,
                GameId = gameId,
                Cards = takenCards
            });
        }
    }

    public class CardsDealed : Event
    {
        public string GameId { get; set; }
        public List<PlayerCard> Cards { get; set; }
    }

    public class PlayerCard
    {
        public string UserId { get; set; }
        public int Position { get; set; }
        public Card Card { get; set; }
    }

    public class GameFinished : Event
    {
    }
}