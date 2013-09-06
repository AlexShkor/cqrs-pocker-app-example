using PAQK.Domain.Aggregates.Game.Events;
using PAQK.Hubs;
using PAQK.Platform.Dispatching.Interfaces;

namespace PAQK.Handlers.SingleUseEventHandlers.SignalR
{
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