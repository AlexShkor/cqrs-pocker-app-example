using AKQ.Domain;
using AKQ.Domain.EventHandlers;
using AKQ.Domain.Messaging;
using AKQ.Domain.Services;
using AKQ.Web.App_Start.DependencyResolution;
using Microsoft.Practices.ServiceLocation;
using StructureMap;

namespace AKQ.Tests
{
    public class ContainerSetUp
    {
        public void SetUp()
        {
            var container = ObjectFactory.Container;
            container.Configure(config =>
            {
                config.For<IRoboBridgeAI>().Singleton().Use<RoboBridgeAI>();
                config.For<GamesManager>().Singleton().Use<GamesManager>();
                config.For<IServiceLocator>().Singleton().Use(new StructureMapDependencyResolver(container));
                config.For<IBridgeGameCallback>().Use<BridgeGameCallback>();
                config.For<IEventHandler>().Singleton().Use<TestHandler>();
            });
        }
    }
}