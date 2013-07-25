using System;
using System.Collections.Generic;
using System.Linq;
using AKQ.Domain.Documents;
using AKQ.Domain.Infrastructure.Mongodb;
using AKQ.Domain.Infrastructure.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AKQ.Domain.Services
{
    public class TournamentDocumentsService : ViewService<TournamentDocument>
    {
        public TournamentDocumentsService(MongoDocumentsDatabase database) : base(database)
        {
        }

        protected override MongoCollection<TournamentDocument> Items
        {
            get { return Database.Tournaments; }
        }

        public IEnumerable<TournamentDocument> GetActual()
        {
            return Items.Find(Query<TournamentDocument>.GT(x => x.ExpectedFinishAt, DateTime.Now));
        }

        public IEnumerable<TournamentDocument> GetFinished(string userId)
        {
            return Items.Find(Query<TournamentDocument>.ElemMatch(x => x.Players, builder => builder.And(builder.EQ(x => x.UserId, userId), builder.NE(x=> x.TournamentFinished,null)))).SetSortOrder(SortBy<TournamentDocument>.Descending(x=> x.StartTime));
        }

        public TournamentDocument GetLastNotStarted()
        {
            return Items.Find(Query<TournamentDocument>.GT(x => x.StartTime, DateTime.Now)).LastOrDefault();
        }
    }
}

