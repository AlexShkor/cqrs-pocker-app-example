using System;
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
            ValidateState(_aggregate);
            ValidateEvents();
        }

        public virtual void ValidateState(T a)
        {
        }

        public void ValidateEvents(params string[] names)
        {
            var expected = Expected().ToList();
            if (expected.Count != _aggregate.Changes.Count)
            {
                var message = string.Format("Events:\n{0} \n",
                    string.Join("\n", _aggregate.Changes.Select(x => x.GetType().Name)));
                Assert.AreEqual(expected.Count, _aggregate.Changes.Count, message);
            }
            var ignore = IgnoreList.Create(names);
            for (int i = 0; i < _aggregate.Changes.Count; i++)
            {
                Assert.IsTrue(ObjectComparer.AreObjectsEqual(expected[i], _aggregate.Changes[i], ignore));
            }
        }

        protected IEnumerable<TEvent> GetChanges<TEvent>() where TEvent : class
        {
            return _aggregate.Changes.Select(x => x as TEvent).Where(x => x != null);
        }
    }
}