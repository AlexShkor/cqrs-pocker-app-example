using MongoDB.Bson;

namespace PAQK.Infrastructure
{
    public class IdGenerator 
    {
        public string Generate()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}
