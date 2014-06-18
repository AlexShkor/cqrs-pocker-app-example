using System;
using Microsoft.Practices.ServiceLocation;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using Poker.Common.Interceptors;
using Poker.Databases;
using Poker.Platform.Dispatching;
using Poker.Platform.Dispatching.Interfaces;
using Poker.Platform.Domain;
using Poker.Platform.Domain.EventBus;
using Poker.Platform.Domain.Interfaces;
using Poker.Platform.Domain.Transitions.Interfaces;
using Poker.Platform.Domain.Transitions.Mongo;
using Poker.Platform.Mongo;
using Poker.Platform.Settings;
using Poker.Platform.StructureMap;
using Poker.Platform.Upgrade;
using Poker.Views;
using StructureMap;
using Uniform;
using Uniform.Mongodb;

namespace Poker
{
    public class Bootstrapper
    {
        public void Configure(IContainer container, bool isReplayMode = false)
        {
            ConfigureSettings(container);
            ConfigureMongoDb(container);
            ConfigureTransport(container, isReplayMode);
            ConfigureEventStore(container);
            ConfigureUniform(container);
            ConfigureUpgrade(container);
            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(container));
        }

        public void ConfigureSettings(IContainer container)
        {
            var settings = SettingsMapper.Map<SiteSettings>();
            container.Configure(config => config.For<SiteSettings>().Singleton().Use(settings));
        }

        public void ConfigureMongoDb(IContainer container)
        {
            // Register bson serializer conventions
            var myConventions = new ConventionPack
            {
                new NoDefaultPropertyIdConvention(),
                new IgnoreExtraElementsConvention(true),
            };
            ConventionRegistry.Register("MyConventions", myConventions, type => true);

            BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeSerializationOptions.UtcInstance));
            BsonSerializer.RegisterSerializer(typeof(DateTime?), new NullableSerializer<DateTime>());

            RegisterBsonMaps();

            var settings = container.GetInstance<SiteSettings>();
            container.Configure(config =>
            {
                config.For<MongoViewDatabase>().Singleton().Use(new MongoViewDatabase(settings.MongoViewConnectionString).EnsureIndexes());
                config.For<MongoLogsDatabase>().Singleton().Use(new MongoLogsDatabase(settings.MongoLogsConnectionString).EnsureIndexes());
                config.For<MongoEventsDatabase>().Singleton().Use(new MongoEventsDatabase(settings.MongoEventsConnectionString));
                config.For<MongoAdminDatabase>().Singleton().Use(new MongoAdminDatabase(settings.MongoAdminConnectionString));
            });
        }

        private static void RegisterBsonMaps()
        {
            //BsonClassMap.RegisterClassMap<UserView>();
        }

        public void ConfigureEventStore(IContainer container)
        {
            var settings = container.GetInstance<SiteSettings>();
            var dispatcher = container.GetInstance<IDispatcher>();

            var transitionsRepository = new MongoTransitionRepository(settings.MongoEventsConnectionString);

            container.Configure(config =>
            {
                config.For<ITransitionRepository>().Singleton().Use(transitionsRepository);
                config.For<IEventBus>().Singleton().Use(new DispatcherEventBus(dispatcher));
                config.For<IRepository>().Use<Repository>();
                config.For(typeof(IRepository<>)).Use(typeof(Repository<>));
            });
        }

        public void ConfigureUniform(IContainer container)
        {
            var settings = container.GetInstance<SiteSettings>();

            // 1. Create databases
            var mongodbDatabase = new MongodbDatabase(settings.MongoViewConnectionString);

            // 2. Configure uniform 
            var uniform = UniformDatabase.Create(config => config
                .RegisterDocuments(typeof(UserView).Assembly)
                .RegisterDatabase(ViewDatabases.Mongodb, mongodbDatabase));

            container.Configure(config => config.For<UniformDatabase>().Singleton().Use(uniform));
        }

        private void ConfigureTransport(IContainer container, bool isReplayMode)
        {
            var namespaces = isReplayMode
                // Only View and Index handlers are used when replaying
                ? new[] { "Poker.Handlers.ViewHandlers", "Poker.Handlers.ViewHandlers" }
                // but all handlers are used in standard mode
                : new string[] { };

            var dispatcher = Dispatcher.Create(d => d
                    .AddHandlers(typeof(UserView).Assembly, namespaces)
                    .AddInterceptor(typeof(LoggingInterceptor))
                    .SetServiceLocator(new StructureMapServiceLocator(container)));

            container.Configure(config =>
            {
                config.For<ICommandBus>().Use<CommandBus>();
                config.For<IDispatcher>().Singleton().Use(dispatcher);
            });
        }

        private void ConfigureUpgrade(IContainer container)
        {
            container.Configure(config => config.Scan(scanner =>
            {
                scanner.AssemblyContainingType<IUpgrader>();
                scanner.AddAllTypesOf<IUpgrader>();
            }));
        }
    }
}
