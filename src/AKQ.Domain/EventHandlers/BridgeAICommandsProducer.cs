using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using AKQ.Domain.GameEvents;
using AKQ.Domain.Messaging;
using AKQ.Domain.Services;
using AKQ.Domain.UserEvents;
using MongoDB.Bson;
namespace AKQ.Domain.EventHandlers
{

    [ProcessingOrder(QueueNamesEnum.AI)]
    public sealed class BridgeAICommandsProducer : BridgeEventHandler
    {
        private readonly GamesManager _gamesManager;
        private static ConcurrentDictionary<string, string> _robots = new ConcurrentDictionary<string, string>();

        static BridgeAICommandsProducer()
        {
            _robots["sally01"] = "Sally";
            _robots["steve01"] = "Steve";
        }

        private readonly IRoboBridgeAI _aiService;
        private readonly IRoboBridgeAI _ddsAiService;

        public BridgeAICommandsProducer(GamesManager gamesManager, IRoboBridgeAI aiService)
        {
            _gamesManager = gamesManager;
            _aiService = aiService;
            _ddsAiService = new DDSRemoteAI();
            AddHandler<PlayerTurn>(e =>
            {
                if (e.IsAI || _robots.ContainsKey(e.UserId))
                {
                    PlayCurrentTurn(e.GameId,e.UserId, e.RoboBridgePBN, e.Player, e.CurrentOrPartner, e.TrickNumber, e.UseDDS, e.GamePBN);
                }
            });
            AddHandler<GameStarted>(e =>
            {
                if (e.TournamentInfo != null && e.TournamentInfo.TableNumber == 1 && !_robots.ContainsKey(e.HostId))
                {
                    Thread.Sleep(5000);
                    JoinTournamentGame("sally01", e.TournamentInfo.Id);
                    Thread.Sleep(5000);
                    JoinTournamentGame("steve01", e.TournamentInfo.Id);
                }

            });
            AddHandler<GameCreated>(e =>
            {
                if (_robots.ContainsKey(e.HostId))
                {
                    Thread.Sleep(5000);
                    _gamesManager.Do(new StartGame
                    {
                        GameId = e.GameId,
                        UserId = e.HostId
                    });
                }
            });
            AddHandler<GameFinished>(e =>
            {
                if (e.TournamentInfo != null && !e.TournamentInfo.IsLastGame && _robots.ContainsKey(e.HostId))
                {
                    Thread.Sleep(5000);
                    JoinTournamentGame(e.HostId, e.TournamentInfo.Id);
                }
            });
        }

        private void PlayCurrentTurn(string gameId, string userId, string roboBridgePBN, PlayerPosition current, PlayerPosition askedPlayer, int trickNumber, bool userDds, string gamePBN)
        {
            var card = userDds || trickNumber >= 11
                           ? _ddsAiService.GetCard(gamePBN, askedPlayer)
                           : _aiService.GetCard(roboBridgePBN, askedPlayer);
            _gamesManager.Do(new PlayCard
            {
                GameId = gameId,
                Card = Card.FromString(card),
                Player = current,
                UserId = userId
            });
        }

        private void JoinTournamentGame(string robotId, string tournamentId)
        {
            _gamesManager.Do(new CreateGame
            {
                GameId = ObjectId.GenerateNewId().ToString(),
                UserId = robotId,
                UserName = _robots[robotId],
                DealId = null,
                GameMode = GameModeEnum.Tournament,
                TournamentId = tournamentId
            });
        }
    }
}