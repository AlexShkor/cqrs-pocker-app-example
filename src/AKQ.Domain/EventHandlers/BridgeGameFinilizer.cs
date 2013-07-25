using System;
using System.Threading;
using AKQ.Domain.GameEvents;
using AKQ.Domain.Messaging;
using AKQ.Domain.UserEvents;

namespace AKQ.Domain.EventHandlers
{
    [ProcessingOrder(QueueNamesEnum.Database, Order = 4)]
    public sealed class BridgeGameFinilizer : BridgeEventHandler
    {
        private readonly GamesManager _gamesManager;

        public BridgeGameFinilizer(GamesManager gamesManager)
        {
            _gamesManager = gamesManager;
            AddHandler((GameFinished e) =>
            {
                Thread.Sleep(5000);
                _gamesManager.FinilizeGame(e.GameId);
            });
        }
    }
}