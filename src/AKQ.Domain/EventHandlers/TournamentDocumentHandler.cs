using System;
using AKQ.Domain.Documents;
using AKQ.Domain.Messaging;
using AKQ.Domain.Services;
using AKQ.Domain.UserEvents;

namespace AKQ.Domain.EventHandlers
{
    [ProcessingOrder(QueueNamesEnum.Database, Order = 3)]
    public sealed class TournamentDocumentHandler : BridgeEventHandler
    {
        private readonly TournamentDocumentsService _documents;
        private readonly BridgeGameDocumentsService _games;
        private readonly BridgeDealService _bridgeDealService;

        public TournamentDocumentHandler(TournamentDocumentsService documents, BridgeGameDocumentsService games, BridgeDealService bridgeDealService)
        {
            _documents = documents;
            _games = games;
            _bridgeDealService = bridgeDealService;
            AddHandler<GameStarted>(e =>
            {
                if (e.TournamentInfo == null)
                {
                    return;
                }
                var game = _games.GetById(e.GameId);
                var tournament = _documents.GetById(e.TournamentInfo.Id);
                var tournamentGameInfo = new TournamentGameInfo
                {
                    Contract = new ContractDocument(e.Contract),
                    GameId = e.GameId,
                    Started = e.TournamentInfo.TournamentStarted.Value,
                    UserId = game.HostUserId,
                    UserName = game.HostUserName,
                    DealId = game.DealId
                };
                if (tournament != null)
                {
                    tournament.AddGameInfo(tournamentGameInfo);
                }
                else
                {
                    tournament = new TournamentDocument(e.TournamentInfo.Id, e.TournamentInfo.HandsToPlay,
                                                        e.TournamentInfo.MinutesToPlay, tournamentGameInfo);
                    for (int i = 1; i < e.TournamentInfo.HandsToPlay; i++)
                    {
                        var nextDeal = _bridgeDealService.GetRandomDeal(DealTypeEnum.Attack);
                        tournament.Deals.Add(nextDeal.Id);
                    }
                }
                _documents.Save(tournament);
            });
            AddHandler<GameFinished>(e =>
            {
                if (e.TournamentInfo == null)
                {
                    return;
                }
                _documents.Update(e.TournamentInfo.Id, (doc) => doc.SetGameResults(e.GameId, e.Result, e.Finished));
            });
        }
    }
}