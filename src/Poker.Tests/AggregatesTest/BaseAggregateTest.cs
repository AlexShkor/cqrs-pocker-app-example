using System;
using System.Collections.Generic;
using Poker.Platform.Domain;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Tests.AggregatesTest
{
    public class BaseAggregateTest
    {
    }

    public class FakeRepository<T> : IRepository<T> where T : Aggregate
    {
        private readonly Dictionary<string, T> _items = new Dictionary<string, T>();

        public void Save(string aggregateId, T aggregate)
        {
            _items.Add(aggregateId,aggregate);
        }

        public T GetById(string id)
        {
            return _items[id];
        }

        public void Perform(string id, Action<T> action)
        {
            action(_items[id]);
        }
    }
}
