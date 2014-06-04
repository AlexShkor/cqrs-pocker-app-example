using System.Linq;
using Poker.Databases;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Hubs;
using Poker.Platform.Dispatching;
using Poker.Platform.Dispatching.Attributes;
using Poker.Platform.Dispatching.Interfaces;

namespace Poker.Handlers.SingleUseEventHandlers.SignalR
{
    [Priority(PriorityStages.ViewHandling_After)]
    public class GameHubEventHandler : IMessageHandler
    {
        private readonly ViewDatabase _db;

        public GameHubEventHandler(ViewDatabase db)
        {
            _db = db;
        }

        public void Handle(NextPlayerTurned e)
        {
            UsersHub.CurrentContext.Clients.Group(e.Id).playerTurnChanged(new
            {
                TableId = e.Id,
                CurrentPlayerId = e.Player.UserId,
            });
        }

        public void Handle(BidMade e)
        {
            var table = _db.Tables.GetById(e.Id);
            var maxBid = table.Players.Select(x => x.Bid).Max();
            UsersHub.CurrentContext.Clients.Group(e.Id).bidMade(new
            {
                TableId = e.Id,
                UserId = e.Bid.UserId,
                NewCashValue = e.Bid.NewCashValue,
                Bid = e.Bid.Bid,
                MaxBid = maxBid
            });
        }
    }
}