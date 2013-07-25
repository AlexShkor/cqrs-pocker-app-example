using System;
using AKQ.Domain.Infrastructure.Mongodb;
using AKQ.Domain.Infrastructure.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AKQ.Domain.Services
{
    public class ConnectionsViewService : ViewService<ConnectionView>
    {
        public ConnectionsViewService(MongoDocumentsDatabase database)
            : base(database)
        {
        }

        protected override MongoCollection<ConnectionView> Items
        {
            get { return Database.Connections; }
        }

        public ConnectionView PopOne()
        {
            var item = Items.FindOneAs<ConnectionView>();
            if (item != null)
            {
                RemoveById(item.ConntectionId);
            }
            return item;
        }

        public void RemoveById(string connectionId)
        {
            Items.Remove(Query.EQ("_id", connectionId));
        }
    }

    public class ConnectionView
    {
        [BsonId]
        public string ConntectionId { get; set; }

        public string Name { get; set; }

        public DateTime ConnectedDateTime { get; set; }

        public ConnectionView(string connectionId)
        {
            ConntectionId = connectionId;
            ConnectedDateTime = DateTime.UtcNow;
            Name = "NoName";
        }

    }
}