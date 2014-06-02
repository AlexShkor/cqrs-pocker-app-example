using Poker.Domain.Aggregates.Game.Events;
using Poker.Hubs;
using Poker.Platform.Dispatching;
using Poker.Platform.Dispatching.Attributes;
using Poker.Platform.Dispatching.Interfaces;

namespace Poker.Handlers.SingleUseEventHandlers.SignalR
{
    [Priority(PriorityStages.ViewHandling_After)]
    public class TableHubEventHandler : IMessageHandler
    {
        public void Handle(PlayerJoined e)
        {
            UsersHub.CurrentContext.Clients.Group(e.UserId).goToTable(new
            {
                TableId = e.Id
            });
        }
    }
}