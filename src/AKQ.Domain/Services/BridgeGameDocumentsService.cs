using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AKQ.Domain.Documents;
using AKQ.Domain.Infrastructure.Mongodb;
using AKQ.Domain.Infrastructure.Mvc;
using AttributeRouting.Helpers;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AKQ.Domain.Services
{
    public class BridgeGameDocumentsService : ViewServiceFiltered<BridgeGameDocument, BridgeGameFilter>
    {
        public BridgeGameDocumentsService(MongoDocumentsDatabase database) : base(database)
        {
        }

        protected override MongoCollection<BridgeGameDocument> Items
        {
            get { return Database.BridgeGames; }
        }

        public IEnumerable<BridgeGameDocument> GetLastWeek(string userId)
        {
            return Items.FindAs<BridgeGameDocument>(
                Query.And(
                Query<BridgeGameDocument>.EQ(x => x.HostUserId, userId),
                Query<BridgeGameDocument>.GTE(x => x.Created, DateTime.UtcNow.AddDays(-7)),
                Query<BridgeGameDocument>.NE(x => x.Finished, null)
                )
                ).SetSortOrder(SortBy<BridgeGameDocument>.Descending(x => x.Created));
        }

        public IEnumerable<BridgeGameDocument> GetByDealId(string dealId)
        {
            return Items.Find(
                Query.And(
                    Query<BridgeGameDocument>.EQ(x => x.DealId, dealId),
                    Query<BridgeGameDocument>.NE(x => x.Finished, null)
                    ));
        }

        public IEnumerable<BridgeGameDocument> GetByDealIdForUser(string dealId, string userId)
        {
            return Items.Find(
                Query.And(
                    Query<BridgeGameDocument>.EQ(x => x.HostUserId, userId),
                    Query<BridgeGameDocument>.EQ(x => x.DealId, dealId),
                    Query<BridgeGameDocument>.NE(x => x.Finished, null)
                    ));
        }

        public IEnumerable<BridgeGameDocument> GetNotStarted()
        {
            return Items.Find(
                Query.And(
                    Query<BridgeGameDocument>.EQ(x => x.Started, null),
                    Query<BridgeGameDocument>.GTE(x => x.Created, DateTime.Now.AddHours(-12))
                    )).SetSortOrder(SortBy<BridgeGameDocument>.Descending(x=> x.Created));
        }

        protected override IEnumerable<IMongoQuery> BuildFilterQuery(BridgeGameFilter filter)
        {
            if (filter.DealType.HasValue)
            {
                yield return Query<BridgeGameDocument>.EQ(x => x.GameType, filter.DealType.Value);
            }
            if (filter.HostId.HasValue())
            {
                yield return Query<BridgeGameDocument>.EQ(x => x.HostUserId, filter.HostId);
            }
            if (filter.Finished.HasValue)
            {
                if (filter.Finished == true)
                {
                    yield return Query<BridgeGameDocument>.NE(x => x.Finished, null);
                }
                else
                {
                    yield return Query<BridgeGameDocument>.EQ(x => x.Finished, null);
                }
            }
            if (filter.Won.HasValue)
            {
                if (filter.Won == true)
                {
                    yield return Query<BridgeGameDocument>.GTE(x => x.Result, 0);
                }
                else
                {
                    yield return Query<BridgeGameDocument>.LT(x => x.Result, 0);
                }
            }
        }
    }

    public class BridgeGameFilter : BaseFilter
    {
        public DealTypeEnum? DealType { get; set; }
        public bool? Won{ get; set; }

        public string HostId { get; set; }

        public bool? Finished { get; set; }
    }
}
