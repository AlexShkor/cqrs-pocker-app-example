using System;
using System.Collections.Generic;
using System.Linq;
using AKQ.Domain.Documents;
using AKQ.Domain.Infrastructure.Mongodb;
using AKQ.Domain.Infrastructure.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace AKQ.Domain.Services
{
    public class BridgeDealService : ViewServiceFiltered<BridgeDeal, BaseFilter>
    {
        public BridgeDealService(MongoDocumentsDatabase database)
            : base(database)
        {
        }

        protected override MongoCollection<BridgeDeal> Items
        {
            get { return Database.BridgeDeals; }
        }

        protected IMongoQuery MandatoryQuery
        {
            get
            {
                return Query.Or(Query<BridgeDeal>.EQ(x => x.BestContract.Position, "S"),
                                Query<BridgeDeal>.EQ(x => x.BestContract.Position, "E"));
            }
        }

        protected override IEnumerable<IMongoQuery> BuildFilterQuery(BaseFilter filter)
        {
            yield break;
        }

        public long GetDealsCount(DealTypeEnum? dealType = null)
        {
            if (dealType.HasValue)
            {
                return GetByQuery(Query<BridgeDeal>.EQ(x => x.DealType, dealType)).Count();
            }
            else
            {
                return Items.Count(MandatoryQuery);
            }
        }

        public BridgeDeal GetRandomDeal(DealTypeEnum? dealType = null)
        {
            var total = GetDealsCount(dealType);
            if(total == 0)
                throw new Exception("Please upload PBN on admin.");

            var rand = new Random(); // TODO: @Andrew: make random static! Use google if have questions
            var r = rand.Next(0, (int)total);
            //TODO: bad random function, it iterate from 0 to r. 

            if (dealType.HasValue)
            {
                return GetByQuery(Query<BridgeDeal>.EQ(x=> x.DealType,dealType)).SetSkip(r).SetLimit(1).First();
            }
            else
            {
                return Items.Find(MandatoryQuery).SetSkip(r).SetLimit(1).First();
            }
        }

        public BridgeDeal GetByBoardNumber(string id)
        {
            return Items.FindOne(Query<BridgeDeal>.EQ(x => x.BoardNumber, id));
        }

        public IEnumerable<BridgeDeal> Get(DealTypeEnum gameType, int skip, int count)
        {
            return GetByQuery(Query<BridgeDeal>.EQ(x => x.DealType, gameType)).SetSortOrder(SortBy<BridgeDeal>.Ascending(x=> x.Id)).SetSkip(skip).SetLimit(count);
        }


        private MongoCursor<BridgeDeal> GetByQuery(IMongoQuery query)
        {
            if (query == null)
            {
                return Items.Find(MandatoryQuery);
            }
            return Items.Find(Query.And(MandatoryQuery, query));
        }
    }
}
