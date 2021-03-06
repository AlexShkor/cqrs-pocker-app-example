using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Poker.Databases;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Platform.Logging
{
    public class LogManager
    {
        public MongoLogsDatabase Logs;

        public LogManager(MongoLogsDatabase logs)
        {
            Logs = logs;
        }

        /// <summary>
        /// Log command
        /// </summary>
        public void LogCommand(ICommand command)
        {
            try
            {
                // Initialize command TypeName if not already initialized
                if (String.IsNullOrEmpty(command.Metadata.TypeName))
                    command.Metadata.TypeName = command.GetType().FullName;

                var commandDocument = command.ToBsonDocument();

                // Removing _t element (generated by mongodb driver)
                commandDocument.Remove("_t");

                var record = new BsonDocument
                {
                    { "_id", command.Metadata.CommandId },
                    { "Command", commandDocument },
                    { "Handlers", new BsonArray() },
                    { "Events", new BsonArray() }
                };

                Logs.Logs.Insert(record);
            }
            catch (Exception)
            {
                // Catch all errors because logging should not throw errors if unsuccessful
            }
        }

        public void LogCommandHandler(CommandHandlerRecord record)
        {
            try
            {
                var handlerDoc = CommandHandlerRecord.ToBson(record);
                var query = Query.EQ("_id", record.CommandId);
                var update = Update.Push("Handlers", handlerDoc);

                if (record.ErrorMessage != "")
                    update.Inc("Errors", 1);

                Logs.Logs.Update(query, update);
            }
            catch (Exception)
            {
                // Catch all errors because logging should not throw errors if unsuccessful
            }
        }

        public void LogCommandHandler(String commandId, string handlerTypeName, Exception e = null)
        {
            try
            {
                LogCommandHandler(new CommandHandlerRecord()
                {
                    HandlerId = Guid.NewGuid().ToString(),
                    CommandId = commandId,
                    EndedDate = DateTime.UtcNow,
                    ErrorMessage = e == null ? "" : e.Message,
                    ErrorStackTrace = e == null ? "" : e.ToString(),
                    TypeName = handlerTypeName
                });
            }
            catch (Exception)
            {
                // Catch all errors because logging should not throw errors if unsuccessful
            }
        }

        public void LogEvent(IEvent e)
        {
            try
            {
                // Initialize command TypeName if not already initialized
                if (String.IsNullOrEmpty(e.Metadata.TypeName))
                    e.Metadata.TypeName = e.GetType().FullName;

                var eventDocument = e.ToBsonDocument();

                // Removing _t element (generated by mongodb driver)
                eventDocument.Remove("_t");

                var eventRecord = new BsonDocument
                {
                    { "Event", eventDocument },
                    { "Handlers", new BsonArray() },
                };

                var query = Query.And(
                    Query.EQ("_id", e.Metadata.CommandId),
                    Query.NE("Events.Event.Metadata.EventId", e.Metadata.EventId)
                );

                var update = Update.Push("Events", eventRecord);
                Logs.Logs.Update(query, update);
            }
            catch (Exception)
            {
                // Catch all errors because logging should not throw errors if unsuccessful
            }
        }

        public void LogEventHandler(EventHandlerRecord record)
        {
            try
            {
                var handlerDoc = EventHandlerRecord.ToBson(record);

                var query = Query.And(
                    Query.EQ("_id", record.CommandId),
                    Query.EQ("Events.Event.Metadata.EventId", record.EventId)
                );

                var update = Update.Push("Events.$.Handlers", handlerDoc);

                if (record.ErrorMessage != "")
                    update.Inc("Errors", 1);

                Logs.Logs.Update(query, update);
            }
            catch (Exception)
            {
                // Catch all errors because logging should not throw errors if unsuccessful
            }
        }

        public void LogEventHandler(String commandId, string eventId, string handlerTypeName, Exception exception = null)
        {
            try
            {
                LogEventHandler(new EventHandlerRecord()
                {
                    HandlerId = Guid.NewGuid().ToString(),
                    CommandId = commandId,
                    EventId = eventId,
                    EndedDate = DateTime.UtcNow,
                    ErrorMessage = exception == null ? "" : exception.Message,
                    ErrorStackTrace = exception == null ? "" : exception.ToString(),
                    TypeName = handlerTypeName
                });
            }
            catch (Exception)
            {
                // Catch all errors because logging should not throw errors if unsuccessful
            }
        }

        public List<LogRecord> BsonToRecords(List<BsonDocument> documents)
        {
            var records = new List<LogRecord>();
            foreach (var document in documents)
            {
                var record = LogRecord.FromBson(document);
                records.Add(record);
            }

            return records;
        }

        /// <summary>
        /// Load record by command ID
        /// </summary>
        public LogRecord GetRecordByCommandId(String commandId)
        {
            var recordDoc = Logs.Logs.FindOneAs<BsonDocument>(Query.EQ("_id", commandId));
            return LogRecord.FromBson(recordDoc);
        }
    }
}
