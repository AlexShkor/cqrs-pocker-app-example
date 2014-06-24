using Poker.Databases;
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
        private readonly ViewDatabase _db;

        public TableHubEventHandler(ViewDatabase db)
        {
            _db = db;
        }

        public void Handle(PlayerJoined e)
        {
            UsersHub.CurrentContext.Clients.Group(e.UserId).goToTable(new
            {
                TableId = e.Id
            });

            var table = _db.Tables.GetById(e.Id);
            UsersHub.CurrentContext.Clients.All.updateTable(new
            {
                Table = table
            });
        }


        public void Handle(TableCreated e)
        {
            UsersHub.CurrentContext.Clients.Group(e.Metadata.UserId).goToTablesView();
        }
    }
}