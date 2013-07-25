using AKQ.Domain.Documents;
using AKQ.Domain.Documents.Progress;
using AKQ.Domain.Infrastructure.Mongodb;
using AKQ.Domain.Infrastructure.Mvc;
using MongoDB.Driver;

namespace AKQ.Domain.Services
{
    public class UserProgressService : ViewService<UserProgress>
    {
        public UserProgressService(MongoDocumentsDatabase database) : base(database)
        {
        }

        protected override MongoCollection<UserProgress> Items
        {
            get { return Database.UserProgress; }
        }
    }
}