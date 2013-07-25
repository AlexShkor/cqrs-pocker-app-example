using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AKQ.Domain;
using AKQ.Domain.Documents;
using AKQ.Domain.PBN;
using AKQ.Domain.Services;
using AKQ.Web.Controllers.ActionResults;
using AKQ.Web.Models;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using DdsContract;
using RestSharp.Extensions;

namespace AKQ.Web.Controllers
{
    [RoutePrefix("replay")]
    public class ReplayController : BaseController
    {
        private readonly BridgeGameDocumentsService _games;
        private readonly UsersService _users;
        private readonly TournamentDocumentsService _tournaments;
        private readonly BestPlaysService _bestPlays;
        private readonly BridgeDealService _bridgeDealService;

        public ReplayController(BridgeGameDocumentsService games, TournamentDocumentsService tournaments, UsersService users, BestPlaysService bestPlays, BridgeDealService bridgeDealService)
        {
            _games = games;
            _tournaments = tournaments;
            _users = users;
            _bestPlays = bestPlays;
            _bridgeDealService = bridgeDealService;
        }

        [POST("others")]
        public ActionResult OthersForGame(string gameId)
        {
            var game = _games.GetById(gameId);
            var others = _games.GetByDealId(game.DealId).OrderByDescending(x => x.Result).ThenByDescending(x => x.Contract.Value).ThenByDescending(x => x.Contract.Suit);
            var model = others.Select(x => new OtherGameViewModel(x)).ToList();
            return new JsonNetResult(model);
        }

        [POST("tournament/others")]
        public ActionResult OthersForTournament(string tournamentId, string gameId)
        {
            var tournament = _tournaments.GetById(tournamentId);
            var game = _games.GetById(gameId);
            var others =
                tournament.Players.Where(x => x.IsFinishedInTime == true).Select(
                    x => x.Games.Find(g => g.DealId == game.DealId))
           // var others = _games.GetByDealId(game.DealId, gameId).
                .OrderByDescending(x => x.Result).ThenByDescending(x => x.Contract.Value).ThenByDescending(x => x.Contract.Suit);
            var model = others.Select(x => new OtherGameViewModel(x)).ToList();
            return new JsonNetResult(model);
        }


        [POST("myhands")]
        public ActionResult MyHands(string tournamentId, string gameId)
        {
            var myhands = new List<OtherGameViewModel>();
            if (tournamentId.HasValue())
            {
                var tournament = _tournaments.GetById(tournamentId);
                myhands.AddRange(tournament.Players.Find(x=> x.UserId == UserId).Games.Select(g=> new OtherGameViewModel(g)));
            }
            else
            {
                var game = _games.GetById(gameId);
                myhands.Add(new OtherGameViewModel(game));
            }
            return new JsonNetResult(myhands);
        }


        [POST("update-tags")]
        public ActionResult UpdateTags(TacticsViewModelPost model)
        {

            var user = _users.GetById(UserId);
            var item = new DealTags()
            {
                DealId = model.DealId,
                Tags = model.Tags.Select(x => new Tag
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList()
            };
            user.UpdateTags(item);
            _users.Save(user);
            return new JsonNetResult(model);
        }


        [POST("best-play")]
        public ActionResult BestPlay(string dealId)
        {
            var bestPlay = _bestPlays.GetById(dealId);
            if (bestPlay == null)
            {
                var dds = new DdsApiClient();
                var deal = _bridgeDealService.GetById(dealId);
                var requestedPbn = deal.GetPBN();
                var ddsResult = dds.PlayGame(requestedPbn);
                bestPlay = CreateBridgeGameDocument(deal, requestedPbn, ddsResult);
                _bestPlays.Save(bestPlay);
            }
            var user = _users.GetById(UserId);
            var model = new ReplayViewModel(bestPlay,user);
            return new JsonNetResult(model);
        }

        private BridgeGameDocument CreateBridgeGameDocument(BridgeDeal deal,  string requestPbn, PlayGameResponse ddsResult)
        {
            var now = DateTime.Now;
            var game = new BridgeGameDocument();
            game.Id = deal.Id;
            game.DealId = deal.Id;
            game.BoardNumber = deal.BoardNumber;
            game.GameType = deal.DealType;
            game.HostUserId = "dds_best_play";
            game.HostUserName = "DDS Best Play";
            game.Created = now;
            game.Started = now;
            game.Finished = now;
             var contract = deal.BestContract.ToDomainObject();
            game.Contract = new ContractDocument(contract);
            game.PBN = requestPbn + ddsResult.Play;
            var parseResult = PBN.PbnParser.ParseGame(requestPbn);
            var play = PBN.PbnParser.ParsePlay(ddsResult.Play);
            var player = contract.Declarer.GetNextPlayerPosition();
            var index = 0;
            foreach (var cards in play)
            {
                index++;
                var trick = new Trick(contract.Suit);
                for (int i = 0; i < 4; i++)
                {
                    trick.AddCard(player, Card.FromString(cards[player.Index]));
                    player = player.GetNextPlayerPosition();
                }
                player = trick.Winner;
                game.Tricks.Add(new TrickDocument(trick, index));
            }
            game.OridinalHandsPBN = parseResult.Deal;
            var hands = PBN.PbnParser.ParseHands(parseResult.Deal);
            game.Hands = hands.Select(x => new HandDocument(x)).ToList();
            return game;
        }
    }
}
