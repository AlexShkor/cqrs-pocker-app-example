using AKQ.Domain.Infrastructure.Mongodb;

namespace AKQ.Domain.Services
{
    public class BestPlaysService : BridgeGameDocumentsService
    {
        public BestPlaysService(MongoDocumentsDatabase database) : base(database)
        {
        }

        protected override MongoDB.Driver.MongoCollection<Documents.BridgeGameDocument> Items
        {
            get { return Database.BestPlays; }
        }
    }
}