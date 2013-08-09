using MongoDB.Bson;
using PAQK.Databases;
using PAQK.Documents;
using PAQK.Domain.Aggregates.Game;
using PAQK.Domain.Aggregates.Game.Events;
using PAQK.Domain.Aggregates.User.Events;
using PAQK.Platform.Dispatching;
using PAQK.Platform.Dispatching.Attributes;
using PAQK.Platform.Dispatching.Interfaces;
using PAQK.Views;
using Uniform;

namespace PAQK.Handlers.ViewHandlers
{
    [Priority(PriorityStages.ViewHandling)]
    public class TableViewHandler : IMessageHandler
    {
        private readonly ViewDatabase _db;
        private readonly IDocumentCollection<TableView> _tables;

        public TableViewHandler(ViewDatabase db)
        {
            _db = db;
            _tables = db.Tables;
        }

        public void Handle(TableCreated e)
        {
            _tables.Save(new TableView()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = e.Name,
                BuyIn = e.BuyIn,
                SmallBlind = e.SmallBlind,
                Players = 0,
                MaxPlayers = e.MaxPlayers
            });
        }

        public void Handle(TableArchived e)
        {
            _tables.Delete(e.Id);
        }

        public void Handle(PlayerLeft e)
        {
            _tables.Update(e.Id,table => table.Players--);
        }

        public void Handle(PlayerJoined e)
        {
            _tables.Update(e.Id, table => table.Players++);
        }

    }
}