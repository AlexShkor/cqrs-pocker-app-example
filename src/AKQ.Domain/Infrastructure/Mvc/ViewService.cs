using System;
using System.Collections.Generic;
using AKQ.Domain.Infrastructure.Mongodb;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AKQ.Domain.Infrastructure.Mvc
{
    public abstract class ViewService<T>
    {
        protected readonly MongoDocumentsDatabase Database;

        protected ViewService(MongoDocumentsDatabase database)
        {
            Database = database;
        }

        protected abstract MongoCollection<T> Items { get; }

        public T GetById(string id)
        {
            return Items.FindOneById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Items.FindAll();
        }

        public void Save(T item)
        {
            Items.Save(item);
        }

        public void InsertBatch(params T[] items)
        {
            Items.InsertBatch(items);
        }

        public void Update(string gameId, Action<T> action)
        {
            var doc = GetById(gameId);
            action(doc);
            Save(doc);
        }
    }
}
