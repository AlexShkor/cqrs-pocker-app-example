using System.Collections.Generic;
using AKQ.Domain;
using PAQK.Domain.Aggregates.Game.Data;
using PAQK.Domain.Aggregates.Game.Events;
using PAQK.Domain.Aggregates.Site;
using PAQK.Platform.Domain;

namespace PAQK.Domain.Aggregates.Game
{
    public class GameAggregate : Aggregate<GameState>
    {
        public void Create(string id, List<UserPosition> users, string dealerId)
        {
            var pack = new Pack();
            Apply(new GameCreated()
            {
                Id = id,
                Users = users,
                DealerUserId = dealerId,
                Cards = pack.GetAllCards()
            });
        }
    }
}