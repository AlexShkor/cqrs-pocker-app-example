using MongoDB.Bson;

namespace Poker.Platform.Mongo
{
    public class IdGenerator 
    {
        public string Generate()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}
