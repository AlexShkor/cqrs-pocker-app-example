using AKQ.Domain.Infrastructure.Settings;

namespace AKQ.Domain
{
    public class AkqSettings
    {
        [SettingsProperty("mongo.connection_string")]
        public string MongoConnectionString { get; set; }

        [SettingsProperty("FacebookAppId")]
        public string FacebookAppId { get; set; }

        [SettingsProperty("FacebookSecretKey")]
        public string FacebookSecretKey { get; set; }
    }
}
