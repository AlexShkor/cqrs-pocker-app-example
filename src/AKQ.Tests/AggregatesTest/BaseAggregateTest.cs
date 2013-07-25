using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAQK.Platform.Domain;
using PAQK.Platform.Domain.Interfaces;

namespace AKQ.Tests.AggregatesTest
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
