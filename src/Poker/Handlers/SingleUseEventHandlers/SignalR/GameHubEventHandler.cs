using Poker.Domain.Aggregates.Game.Events;
using Poker.Hubs;
using Poker.Platform.Dispatching.Interfaces;

namespace Poker.Handlers.SingleUseEventHandlers.SignalR
{
    public class GameHubEventHandler : IMessageHandler
    {
        public void Handle(NextPlayerTurned e)
        {
            GameHub.CurrentContext.Clients.Group(e.Id).playerTurnChanged(new
            {
                TableId = e.Id,
                CurrentPlayerId = e.Player.UserId
            });
        }
    }
}