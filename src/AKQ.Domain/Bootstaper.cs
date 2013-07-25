
using AKQ.Domain.Infrastructure.Mongodb;
using AKQ.Domain.Infrastructure.Settings;
using AKQ.Domain.Services;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using StructureMap;

namespace AKQ.Domain
{
    public class Bootstaper
    {
        public void Configure(IContainer container)
        {
            ConfigureSettings(container);
            ConfigureMongoDb(container);
            ConfigureServices(container);
        }

        private void ConfigureServices(IContainer container)
        {
        }

        public void ConfigureSettings(IContainer container)
        {
            var settings = SettingsMapper.Map<AkqSettings>();

            container.Configure(config => config.For<AkqSettings>().Singleton().Use(settings));
        }

        public void ConfigureMongoDb(IContainer container)
        {
            var myConventions = new ConventionProfile();
            myConventions.SetIgnoreExtraElementsConvention(new AlwaysIgnoreExtraElementsConvention());
            BsonClassMap.RegisterConventions(myConventions, type => true);
            DateTimeSerializationOptions.Defaults = DateTimeSerializationOptions.LocalInstance;

            RegisterBsonMaps();

            var settings = container.GetInstance<AkqSettings>();
            container.Configure(config => config.For<MongoDocumentsDatabase>().Singleton().Use(new MongoDocumentsDatabase(settings.MongoConnectionString)));
        }

        private static void RegisterBsonMaps()
        {
        }
    }
}
