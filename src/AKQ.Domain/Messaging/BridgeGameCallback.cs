using System;
using System.Collections.Generic;
using System.Threading;
using AKQ.Domain.Documents;
using AKQ.Domain.GameEvents;
using AKQ.Domain.Services;
using AKQ.Domain.UserEvents;
using AKQ.Domain.ViewModel;

namespace AKQ.Domain.Messaging
{
    public class BridgeGameCallback : IBridgeGameCallback
    {
        private string _gameId;
        private int _version;

        private readonly Dispatcher _dispatcher;

        public bool Disabled { get; set; }

        public BridgeGameCallback(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void SetGameId(string id)
        {
            _gameId = id;
        }

        public void CardPlayed(Card card, PlayerPosition position, bool isLastCardInTrick)
        {
            Publish(new CardPlayed
            {
                Player = position,
                Card = card,
                IsLast = isLastCardInTrick
            });
        }

        public void PlayerTurnStarted(PlayerPosition player, Suit trickSuit, bool isAI, PlayerPosition currentOrPartner, bool isNextTrick, string roboBridgePbn, string userId, int trickNumber, bool useDds, string gamePBN)
        {
            Publish(new PlayerTurn
            {
                Player = player,
                LedSuit = trickSuit,
                IsAI = isAI,
                CurrentOrPartner = currentOrPartner,
                IsNextTrick = isNextTrick,
                RoboBridgePBN = roboBridgePbn,
                UserId = userId,
                TrickNumber = trickNumber,
                UseDDS = useDds,
                GamePBN = gamePBN
            });
        }

        public void TrickEnded(PlayerPosition winner, int trickNumber, Trick trick)
        {
            Publish(new TrickEnded
            {
                Winner = winner,
                TrickNumber = trickNumber,
                Trick = trick
            });
        }

        public void GameFinished(Deck originalDeck, string gamePBN, string roboBridgePBN, int result, TournamentInfo tournamentInfo, DateTime finished, string hostId, string dealId)
        {

            Publish(new GameFinished
            {
                OriginalHands = originalDeck,
                GamePBN = gamePBN,
                Result = result,
                RoboBridgePBN = roboBridgePBN,
                TournamentInfo = tournamentInfo,
                Finished = finished,
                HostId = hostId,
                DealId = dealId
            });
        }

        public void PlayerJoined(PlayerPosition position, string userId, string name)
        {
            Publish(new PlayerJoined
            {
                Position = position,
                UserId = userId,
                Name = name
            });
        }

        public void GameStarted(Contract contract, TournamentInfo tournamentInfo, string hostId)
        {
            Publish(new GameStarted
            {
                Contract = contract,
                TournamentInfo = tournamentInfo,
                HostId = hostId
            });
        }

        public void GameCreated(string hostId, DealTypeEnum gameType, string dealId)
        {
            Publish(new GameCreated
            {
                HostId = hostId,
                GameType = gameType,
                DealId = dealId
            });
        }

        private void Publish<T>(T e) where T : GameEvent
        {
            if (_gameId == null)
            {
                throw new InvalidOperationException("GameId is not set. Event can't ve processed: " + e.GetType().Name);
            }
            e.GameId = _gameId;
            e.Version = Interlocked.Increment(ref _version);
            _dispatcher.Dispatch(e);
        }
    }
}
