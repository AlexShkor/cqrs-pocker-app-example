using MongoDB.Bson.Serialization.Attributes;
using Uniform;

namespace PAQK.Documents
{
    public class TableView
    {
        [BsonId, DocumentId]
        public string Id { get; set; }

        public string Name { get; set; }

        public int Players { get; set; }

        public long BuyIn { get; set; }

        public long SmallBlind { get; set; }
        public int MaxPlayers { get; set; }
    }
}