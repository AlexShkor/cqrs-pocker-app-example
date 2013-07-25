using System;
using System.Linq;
using AKQ.Domain.Documents;
using AKQ.Domain.GameEvents;
using AKQ.Domain.Messaging;
using AKQ.Domain.Services;
using AKQ.Domain.UserEvents;
using PBN;

namespace AKQ.Domain.EventHandlers
{
    [ProcessingOrder(QueueNamesEnum.Database, Order = 0)]
    public sealed class BridgeGameDocumentHandler : BridgeEventHandler
    {
        private readonly BridgeGameDocumentsService _documents;
        private readonly BridgeDealService _deals;
        private readonly UsersService _users;

        public BridgeGameDocumentHandler(BridgeGameDocumentsService documents, BridgeDealService deals, UsersService users)
        {
            _documents = documents;
            _deals = deals;
            _users = users;
            AddHandler<GameCreated>(e =>
            {
                var deal = _deals.GetById(e.DealId);
                var user = _users.GetById(e.HostId);
                var userName = user != null ? user.Username : "Guest";
                var game = new BridgeGameDocument
                {
                    Id = e.GameId,
                    Created = DateTime.UtcNow,
                    OridinalHandsPBN = deal.PBNHand,
                    BoardNumber = deal.BoardNumber,
                    HostUserId = e.HostId,
                    HostUserName = userName,
                    DealId = e.DealId,
                    GameType = e.GameType,
                    //Hands = PbnParser.ParseHands(deal.PBNHand).Select(hand => new HandDocument(hand)).ToList()
                };
                game.JoinedPlayers.Add(new PlayerDocument(PlayerPosition.South,e.HostId,userName));
                _documents.Save(game);
            });
            AddHandler<GameStarted>(e => _documents.Update(e.GameId, (doc) =>
            {
                doc.Started = DateTime.UtcNow;
                doc.Contract = new ContractDocument(e.Contract);
            }));
            AddHandler<TrickEnded>(e => _documents.Update(e.GameId, (doc) => doc.Tricks.Add(new TrickDocument(e.Trick, e.TrickNumber))));
            AddHandler<GameFinished>(e => _documents.Update(e.GameId, (doc) =>
            {
                doc.Finished = DateTime.UtcNow;
                doc.PBN = e.GamePBN;
                doc.RoboBridgePBN = e.RoboBridgePBN;
                doc.Finished = DateTime.UtcNow;
                doc.Result = e.Result;
            }));
            AddHandler<PlayerJoined>(e => 
                _documents.Update(e.GameId,doc => doc.JoinedPlayers.Add(new PlayerDocument(e.Position, e.UserId, e.Name))));
        }
    }
}