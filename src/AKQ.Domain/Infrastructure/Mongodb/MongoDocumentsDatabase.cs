using System;
using AKQ.Domain.Documents;
using AKQ.Domain.Documents.Progress;
using AKQ.Domain.EventHandlers;
using AKQ.Domain.Services;
using MongoDB.Driver;

namespace AKQ.Domain.Infrastructure.Mongodb
{
    public class MongoDocumentsDatabase
    {
        /// <summary>
        /// MongoDB Server
        /// </summary>
        private readonly MongoServer _server;

        /// <summary>
        /// Name of database 
        /// </summary>
        private readonly string _databaseName;

        public MongoUrl MongoUrl { get; private set; }

        /// <summary>
        /// Opens connection to MongoDB Server
        /// </summary>
        public MongoDocumentsDatabase(String connectionString)
        {
            MongoUrl = MongoUrl.Create(connectionString);
            _databaseName = MongoUrl.DatabaseName;
            _server = new MongoClient(connectionString).GetServer();
            BridgeGames.EnsureIndex("HostUserId");
        }

        /// <summary>
        /// MongoDB Server
        /// </summary>
        public MongoServer Server
        {
            get { return _server; }
        }

        /// <summary>
        /// Get database
        /// </summary>
        public MongoDatabase Database
        {
            get { return _server.GetDatabase(_databaseName); }
        }

        public MongoCollection<User> Users { get { return Database.GetCollection<User>("users"); } }

        public MongoCollection<BridgeDeal> BridgeDeals { get { return Database.GetCollection<BridgeDeal>("bridge_deals"); } }

        public MongoCollection<BridgeDeal> TournamentBridgeDeals { get { return Database.GetCollection<BridgeDeal>("tournament_bridge_deals"); } }

        public MongoCollection<BridgeGameDocument> BridgeGames { get { return Database.GetCollection<BridgeGameDocument>("bridge_games"); } }

        public MongoCollection<ConnectionView> Connections { get { return Database.GetCollection<ConnectionView>("rooms"); } }

        public MongoCollection<TournamentDocument> Tournaments { get { return Database.GetCollection<TournamentDocument>("tournaments"); } }

        public MongoCollection<RobotPlayStatisticDocument> RobotPlayStatistics { get { return Database.GetCollection<RobotPlayStatisticDocument>("robot_stats"); } }

        public MongoCollection<UserProgress> UserProgress { get { return Database.GetCollection<UserProgress>("user_progress"); } }

        public MongoCollection<BridgeGameDocument> BestPlays { get { return Database.GetCollection<BridgeGameDocument>("best_plays"); } }

        public MongoCollection<UserLogDocument> UsersLogs { get { return Database.GetCollection<UserLogDocument>("users_logs"); } }
    }
}
