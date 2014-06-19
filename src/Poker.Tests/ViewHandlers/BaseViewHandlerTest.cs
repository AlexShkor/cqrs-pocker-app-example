using NUnit.Framework;
using Poker.Databases;
using Poker.Domain.Aggregates.Game;
using Poker.Platform.Dispatching;
using Poker.Platform.StructureMap;
using Poker.Views;
using StructureMap;
using Uniform;
using Uniform.InMemory;

namespace Poker.Tests.ViewHandlers
{
    [TestFixture]
    public abstract class BaseViewHandlerTest
    {
        public GameTableAggregate Table { get; set; }
        public ViewDatabase View { get; set; }
        public Dispatcher Dispatcher { get; set; }

        [SetUp]
        public void SetUp()
        {
            IContainer container = ObjectFactory.Container;
            
            Table = new GameTableAggregate();
            Table.Setup(new GameTableState(), 0);

            var db = new InMemoryDatabase();
            var uniform = UniformDatabase.Create(config => config
                .RegisterDocuments(typeof(UserView).Assembly)
                .RegisterDatabase(ViewDatabases.Mongodb, db));
            View = new ViewDatabase(uniform);
            container.Configure(x => x.For<ViewDatabase>().Use(View));

            Dispatcher = Dispatcher.Create(d => d
                   .AddHandlers(typeof(UserView).Assembly, new[] { "Poker.Handlers.ViewHandlers", "Poker.Handlers.ViewHandlers" })
                   .SetServiceLocator(new StructureMapServiceLocator(container)));
        }

        public void SendAllEvents()
        {
            foreach (var @event in Table.Changes)
            {
                Dispatcher.Dispatch(@event);
            }
        }
    }
}