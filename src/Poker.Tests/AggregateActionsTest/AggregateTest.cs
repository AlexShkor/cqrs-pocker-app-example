using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Poker.Platform.Domain;
using Poker.Platform.Domain.Interfaces;
using Poker.Tests.AggregatesTest;

namespace Poker.Tests.AggregateActionsTest
{
    [TestFixture]
    public abstract class AggregateTest<T, TState> where T : Aggregate<TState>, new() where TState : AggregateState, new()
    {
        private T _aggregate;


        [SetUp]
        public void Prepare()
        {
            _aggregate = new T();
            _aggregate.Setup(new TState());
            Given(_aggregate);
            _aggregate.Changes.Clear();
            When(_aggregate);
        }

        public abstract void Given(T a);
        public abstract void When(T a);
        public abstract IEnumerable<IEvent> Expected();


        [Test]
        public virtual void Test()
        {
            Validate();
        }

        public void Validate(params string[] names)
        {
            var expected = Expected().ToList();
            Assert.AreEqual(expected.Count, _aggregate.Changes.Count);
            var ignore = IgnoreList.Create(names);
            for (int i = 0; i < _aggregate.Changes.Count; i++)
            {
                Assert.IsTrue(ObjectComparer.AreObjectsEqual(expected[i], _aggregate.Changes[i], ignore));
            }
        }
    }
}