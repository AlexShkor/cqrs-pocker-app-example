using System;
using AKQ.Domain.Infrastructure.Mongodb;
using AKQ.Domain.Infrastructure.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace AKQ.Domain.Services
{
    public class UserLogsService : ViewService<UserLogDocument>
    {
        public UserLogsService(MongoDocumentsDatabase database) : base(database)
        {
        }

        protected override MongoCollection<UserLogDocument> Items
        {
            get { return Database.UsersLogs; }
        }
    }

    public class UserLogDocument
    {
        [BsonId]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public DateTime Date { get; set; }

        [BsonRepresentation(BsonType.String)]
        public UserLogActionEnum Action { get; set; }
    }

    public enum UserLogActionEnum
    {
        LoggedIn
    }
}