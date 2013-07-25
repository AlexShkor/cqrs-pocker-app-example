using MongoDB.Bson;

namespace AKQ.Domain.Infrastructure
{
    public class IdGenerator 
    {
        public string Generate()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}
