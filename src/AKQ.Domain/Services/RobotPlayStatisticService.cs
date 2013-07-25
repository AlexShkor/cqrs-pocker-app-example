using System;
using AKQ.Domain.Infrastructure.Mongodb;
using AKQ.Domain.Infrastructure.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace AKQ.Domain.Services
{
    public class RobotPlayStatisticService : ViewService<RobotPlayStatisticDocument>
    {
        public RobotPlayStatisticService(MongoDocumentsDatabase database) : base(database)
        {
        }

        protected override MongoCollection<RobotPlayStatisticDocument> Items
        {
            get { return Database.RobotPlayStatistics; }
        }

        public void Append(string gameId, TimeSpan elapsed, int trickNumber, string pbn)
        {
            Items.Insert(new RobotPlayStatisticDocument
            {
                Id = ObjectId.GenerateNewId().ToString(),
                GameId = gameId,
                ElapsedTime = elapsed.ToString(),
                ElapsedMiliseconds = (int)elapsed.TotalMilliseconds,
                PBN = pbn,
                TrickNumber = trickNumber
            });
        }
    }

    public class RobotPlayStatisticDocument
    {
        [BsonId]
        public string Id { get; set; }

        public string ElapsedTime { get; set; }

        public string  PBN { get; set; }

        public string GameId { get; set; }

        public int ElapsedMiliseconds { get; set; }

        public int TrickNumber { get; set; }
    }
}