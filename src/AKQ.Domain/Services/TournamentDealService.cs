using AKQ.Domain.Documents;
using AKQ.Domain.Infrastructure.Mongodb;
using MongoDB.Driver;

namespace AKQ.Domain.Services
{
    public class TournamentDealService: BridgeDealService
    {
        public TournamentDealService(MongoDocumentsDatabase database)
            : base(database)
        {
        }
        protected override MongoCollection<BridgeDeal> Items
        {
            get
            {
                return Database.TournamentBridgeDeals;
            }
        }
    }
}