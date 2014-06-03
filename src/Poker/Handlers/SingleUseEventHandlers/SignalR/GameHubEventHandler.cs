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
            UsersHub.CurrentContext.Clients.Group(e.Id).bidMade(new
            {
                TableId = e.Id,
                Bid = e.Bid
            });
        }
    }
}