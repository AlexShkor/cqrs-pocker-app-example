using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AKQ.Domain.Exceptions;
using AKQ.Domain.Hubs;
using AKQ.Domain.Services;
using AKQ.Domain.Utils;
using AKQ.Domain.ViewModel;

namespace AKQ.Domain
{
    public class GamesManager
    {
        private readonly Dictionary<string, BridgeGame> _games;
        private readonly Dictionary<string, DateTime> _lastAction;
        private readonly Dictionary<string, SemaphoreSlim> _semaphores;

        private readonly BridgeGameFactory _bridgeGameFactory;
        private readonly ConnectionsViewService _connections;
        private readonly ConcurrentQueue<string> _versusGames = new ConcurrentQueue<string>();

        public int GamesCount
        {
            get { return _games.Count; }
        }

        public string PopGame()
        {
            if (_versusGames.Count > 0)
            {
                string gameId;
                while (_versusGames.TryDequeue(out gameId))
                {
                    try
                    {
                        var game = _games[gameId];
                        if (!game.IsStarted)
                        {
                            return gameId;
                        }
                    }
                    catch 
                    {
                    }
                }
            }
            return null;
        }

        public GamesManager(BridgeGameFactory bridgeGameFactory, ConnectionsViewService connections)
        {
            _bridgeGameFactory = bridgeGameFactory;
            _connections = connections;
            _games = new Dictionary<string, BridgeGame>();
            _lastAction = new Dictionary<string, DateTime>();
            _semaphores = new Dictionary<string, SemaphoreSlim>();
        }

        public void Do(CreateGame c)
        {
            Guard.Against(_games.ContainsKey(c.GameId), "Game with the same ID already created.");
            var semaphor = new SemaphoreSlim(1);
            semaphor.Wait();
            _semaphores[c.GameId] = semaphor;
            var game = _bridgeGameFactory.CreateGame(c.GameId, c.GameMode, new GameCreationData(c.DealId, c.UserId, c.UserName, c.TournamentId));
            _games[c.GameId] = game;
            _versusGames.Enqueue(game.Id);
            SendPlayerJoinRequestAsync(game.Id);
            if (game.IsHostedBy(null))
            {
                Do(new StartGame()
                {
                    GameId = game.Id
                });
            }

            _lastAction[c.GameId] = DateTime.UtcNow;
            semaphor.Release();
        }

        private void SendPlayerJoinRequestAsync(string gameId)
        {
            Task.Factory.StartNew(() =>
            {
                var playerToJoin = _connections.PopOne();
                if (playerToJoin != null)
                {
                    RoomHub.AcceptPlayer(playerToJoin.ConntectionId, gameId);
                }
            });
        }

        public void Do(PlayCard c)
        {
            Perform(c.GameId, game =>
            {
                var player = game.PlayersManager.Get(c.Player);
                if (player.HasControl(c.UserId))
                {
                    game.PlayCard(player.PlayerPosition, c.Card);
                }
            });
        }

        public void Do(StartGame c)
        {
            Perform(c.GameId, game =>
            {
                if (!game.Started.HasValue)
                {
                    game.Start(c.UserId);
                }
            });
        }

        public void Do(JoinGame c)
        {
            Perform(c.GameId, game => game.Join(c.UserId, c.UserName, PlayerPosition.East));
        }

        public void Do(MakeBid c)
        {
            Perform(c.GameId, game => game.AddBid(c.Bid));
        }

        private void Perform(string gameId, Action<BridgeGame> action)
        {
            if (!_semaphores.ContainsKey(gameId) || !_games.ContainsKey(gameId))
            {
                throw new BridgeGameNotFound(gameId);
            }
            var game = _games[gameId];
            Task.Factory.StartNew(() =>
            {
                _semaphores[gameId].Wait();
                try
                {
                    action(game);
                    _lastAction[gameId] = DateTime.UtcNow;
                }
                catch
                {
                }
                if (_semaphores.ContainsKey(gameId))
                {
                    _semaphores[gameId].Release();
                }
            });
        }

        public GameViewModel GetGameViewModel(string id, string userId)
        {
            return new GameViewModel(_games[id], userId);
        }

        public void FinilizeGame(string gameId)
        {
            _games.Remove(gameId);
            _semaphores[gameId].Release();
            _semaphores.Remove(gameId);
        }
    }
}