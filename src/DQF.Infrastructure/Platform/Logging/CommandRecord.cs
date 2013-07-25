using MongoDB.Bson;
using PAQK.Platform.Domain.Messages;
using PAQK.Platform.Mongo;

namespace PAQK.Platform.Logging
{
    public class CommandRecord
    {
        public BsonDocument CommandDocument { get; set; }
        public CommandMetadata Metadata { get; set; }
        public CommandHandlerRecordCollection Handlers { get; set; }

        public static CommandRecord FromBson(BsonDocument doc)
        {
            var commandDocument = doc.GetBsonDocument("Command");

            var record = new CommandRecord
            {
                CommandDocument = commandDocument,
                Metadata = commandDocument.GetBsonDocument("Metadata").CreateCommandMetadata(),
                Handlers = CommandHandlerRecordCollection.FromBson(doc.GetBsonArray("Handlers"))
            };

            return record;
        }        
    }
}