using System;
using System.Collections.Generic;
using AKQ.Domain.Documents;

namespace AKQ.Domain.Messaging
{
    public interface IBridgeGameCallback
    {
        bool Disabled { get; set; }
        void CardPlayed(Card card, PlayerPosition position, bool isLastCardInTrick);
        void TrickEnded(PlayerPosition winner, int trickNumber, Trick trick);
        void GameFinished(Deck originalDeck, string gamePBN, string roboBridge, int result, TournamentInfo tournamentInfo, DateTime finished, string hostId, string dealId);
        void PlayerJoined(PlayerPosition position, string userId, string name);
        void GameStarted(Contract contract, TournamentInfo tournamentInfo, string hostId);
        void SetGameId(string id);
        void GameCreated(string hostId, DealTypeEnum gameType, string dealId);
        void PlayerTurnStarted(PlayerPosition currentPosition, Suit trickSuit, bool isAI, PlayerPosition currentOrPartner, bool isNextTrick, string roboBridgePbn, string userId, int trickNumber, bool useDds, string gamePBN);
    }
}