using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace AKQ.Domain.Documents
{
    public class TournamentDocument
    {
        [BsonId]
        public string Id { get; set; }

        public DateTime StartTime { get; set; }

        public int MinutesToPlay { get; set; }

        public int HandsToPlay { get; set; }

        public DateTime? ExpectedFinishAt { get; set; }

        public string HostUserId { get; set; }

        public List<string> Deals{ get; set; }

        public SortedSet<string> RegistredUsers{ get; set; }

        public List<TournamentPlayerInfo> Players { get; set; }

        public TournamentDocument()
        {
            RegistredUsers = new SortedSet<string>();
            Players = new List<TournamentPlayerInfo>();
        }

        public TournamentDocument(string id, string userId, List<string>  deals):this()
        {
            Id = id;
            HostUserId = userId;
            MinutesToPlay = 10;
            HandsToPlay = 3;
            Deals = deals;
            StartTime = DateTime.Now.AddSeconds(20);
            ExpectedFinishAt = StartTime.AddMinutes(MinutesToPlay);
        }

        public TournamentDocument(string id, int handsToPlay, int minutesToPlay, TournamentGameInfo gameInfo)
            : this()
        {
            Id = id;
            StartTime = gameInfo.Started;
            MinutesToPlay = minutesToPlay;
            HandsToPlay = handsToPlay;
            ExpectedFinishAt = gameInfo.Started.AddMinutes(minutesToPlay);
            HostUserId = gameInfo.UserId;
            Deals = new List<string>{gameInfo.DealId};
            var player = new TournamentPlayerInfo(gameInfo);
            Players = new List<TournamentPlayerInfo>()
            {
                player
            };
        }

        public void SetGameResults(string gameId, int result, DateTime finished)
        {

            var player = Players.Find(p => p.Games.Any(g => g.GameId == gameId));
            var game = player.Games.First(x => x.GameId == gameId);
            game.Result = result;
            game.Finished = finished;
            if (player.AllGamesStarted)
            {
                player.AllGamesFinished = true;
                player.TournamentFinished = finished;
                player.IsFinishedInTime = finished <= ExpectedFinishAt;
            }
        }

        public TournamentPlayerInfo GetPlayerInfo(string userId)
        {
            return Players.FirstOrDefault(x => x.UserId == userId);
        }

        public string GetNextDealIdFor(string userId)
        {
            var player = GetPlayerInfo(userId);
            if (player == null)
            {
                return Deals.First();
            }
            if (player.Games.Any(x=> x.Finished == null))
            {
                throw new InvalidOperationException("Can't get new deal till all previous are finished");
            }
            var dealId = Deals[player.NextDealIndex];
            return dealId;
        }

        public void AddGameInfo(TournamentGameInfo tournamentGameInfo)
        {
            var player = GetPlayerInfo(tournamentGameInfo.UserId);
            if (player == null)
            {
                player = new TournamentPlayerInfo(tournamentGameInfo);
                Players.Add(player);
            }
            else
            {
                player.NextDealIndex++;
                player.Games.Add(tournamentGameInfo);
            }
            if (player.Games.Count == Deals.Count)
            {
                player.AllGamesStarted = true;
            }
        }
    }
}