using MongoDB.Bson.Serialization.Attributes;
using Uniform;

namespace PAQK.Views
{
    public class GameView
    {
        [BsonId,DocumentId]
        public string Id { get; set; }
    }
}