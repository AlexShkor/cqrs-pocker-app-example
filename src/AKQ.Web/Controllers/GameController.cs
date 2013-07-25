using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AKQ.Domain;
using AKQ.Domain.Documents;
using AKQ.Domain.Services;
using AKQ.Domain.ViewModel;
using AKQ.Web.Controllers.ActionResults;
using AKQ.Web.Controllers.ModelBinders;
using AKQ.Web.Models;
using AttributeRouting;
using PAQK.Platform.Domain;
using RestSharp.Extensions;
using AttributeRouting.Web.Mvc;
using MongoDB.Bson;
using PBN;

namespace AKQ.Web.Controllers
{
    [RoutePrefix("game")]
    public class GameController : BaseController
    {
        private readonly GamesManager _gamesManager;
        private readonly BridgeGameDocumentsService _bridgeGameDocuments;
        private readonly TournamentDocumentsService _tournaments;
        private readonly UsersService _users;
        private readonly UserProgressService _progress;
        private readonly ICommandBus _bus;

        public GameController(GamesManager gamesManager, BridgeGameDocumentsService bridgeGameDocuments, TournamentDocumentsService tournaments, UsersService users, UserProgressService progress,
            ICommandBus bus)
        {
            _gamesManager = gamesManager;
            _bridgeGameDocuments = bridgeGameDocuments;
            _tournaments = tournaments;
            _users = users;
            _progress = progress;
            _bus = bus;
        }


        [GET("view/{gameId}")]
        public ActionResult Index(string gameId)
        {
            ViewBag.Title = "Game";
            ViewBag.UserName = UserName;
            return View("Templates/Game", (object)gameId);
        }

        [GET("create/{gameType:alpha}")]
        [GET("tournament/{tournamentId}/next")]
        [GET("create/deal/{dealId}")]
        [ValidateInput(false)]
        public ActionResult Create(string gameType, string dealId, string tournamentId)
        {
            if (tournamentId.HasValue())
            {
                var tournament = _tournaments.GetById(tournamentId);
                if (tournament != null)
                {
                    var player = tournament.GetPlayerInfo(UserId);
                    if (player != null && player.Games.Last().Finished  == null)
                    {
                        return RedirectToAction("Index", new {gameId = player.Games.Last().GameId});
                    }
                }
            }
            if (!Request.IsAuthenticated)
            {
                gameType = "random";
            }
            var gameId = ObjectId.GenerateNewId().ToString();
            _gamesManager.Do(new CreateGame
            {
                DealId = dealId != null ? dealId.Replace("_", ":"): null,
                TournamentId = tournamentId,
                UserId = UserId,
                UserName = UserName,
                GameId = gameId,
                GameMode = ParseGameMode(gameType)
            });
            if (Request.IsAjaxRequest())
            {
                return Json(new {gameId});
            }
            return RedirectToAction("Index", new {gameId });
        }

        [POST("create")]
        public JsonNetResult CreateGame(string gameType, string dealId, string tournamentId)
        {
            if (tournamentId.HasValue())
            {
                var tournament = _tournaments.GetById(tournamentId);
                if (tournament != null)
                {
                    var player = tournament.GetPlayerInfo(UserId);
                    if (player != null && player.Games.Last().Finished == null)
                    {
                        return new JsonNetResult(player.Games.Last().GameId);
                    }
                }
            }
            if (!Request.IsAuthenticated)
            {
                gameType = "random";
            }
            var gameId = ObjectId.GenerateNewId().ToString();
            _gamesManager.Do(new CreateGame
            {
                DealId = dealId != null ? dealId.Replace("_", ":") : null,
                TournamentId = tournamentId,
                UserId = UserId,
                UserName = UserName,
                GameId = gameId,
                GameMode = ParseGameMode(gameType)
            });
            return new JsonNetResult(gameId);
        }

        [POST("next-game")]
        public JsonNetResult NextGame(string gameType, string dealId)
        {
            var gameId = ObjectId.GenerateNewId().ToString();
            var progress = _progress.GetById(UserId);
            var currentSet = progress.PracticeProgress.GetCurrentGameSet();
            if (currentSet == null || currentSet.IsFinished())
            {
                dealId = null;
            }
            else
            {
                if (currentSet.Deals.Count(x=> !x.HasWin()) > 1)
                {
                    var deal = currentSet.Deals.First(x => x.DealId != dealId);
                    dealId = deal.DealId;
                }
                else
                {
                    dealId = null;
                }
            }
            _gamesManager.Do(new CreateGame
            {
                DealId = dealId != null ? dealId.Replace("_", ":") : null,
                UserId = UserId,
                UserName = UserName,
                GameId = gameId,
                GameMode = ParseGameMode(gameType)
            });
            return new JsonNetResult(gameId);
        }

        [POST("start")]
        public void Start(string id)
        {
            _gamesManager.Do(new StartGame
            {
                GameId = id,
                UserId = UserId
            });
        }

        [GET("join/{id}")]
        public ActionResult JoinGame(string id)
        {
            _gamesManager.Do(new JoinGame
            {
                UserId = UserId,
                GameId = id,
                UserName = UserName
            });
            return RedirectToAction("Index", new { gameId = id });
        }

        [POST("load")]
        public JsonNetResult GetGame(string id)
        {
            var model = _gamesManager.GetGameViewModel(id,UserId);
            model.IsAuth = Request.IsAuthenticated;
            var user = _users.GetById(UserId);
            model.Tactics = new TacticsViewModel(((user?? new User()) .GetTags(model.DealId)) ?? new List<Tag>());
            return new JsonNetResult(model);
        }

        [POST("play-card")]
        public void PlayCard(string gameId, string suit, string rank, string pos)
        {
            _gamesManager.Do(new PlayCard
            {
                Card = Card.FromString(suit, rank),
                Player = PlayerPosition.FromShortName(pos),
                GameId = gameId,
                UserId = UserId
            });
        }

        [POST("make-bid")]
        public void PlayerBid(string gameId, string bid)
        {
            _gamesManager.Do(new MakeBid
            {
                Bid = Bid.FromString(bid),
                GameId = gameId,
                UserId = UserId
            });
        }

        [GET("replay/{id}")]
        public ActionResult Replay(string id)
        {
            ViewBag.Title = "Replay";
            ViewBag.UserName = UserName;
            return View("Templates/Replay", (object)id);
        }

        [GET("replay/tournament/{tournamentId}", IgnoreRoutePrefix = true)]
        public ActionResult Tournament(string tournamentId)
        {
            var tournament = _tournaments.GetById(tournamentId);
            var gameId = tournament.Players.Find(x => x.UserId == UserId).Games.First().GameId;
            ViewBag.TournamentId = tournamentId;
            ViewBag.UserName = UserName;
            return View("Templates/Replay", (object)gameId);
        }

        [POST("replay")]
        public JsonNetResult LoadReplayModel(string id)
        {
            var doc = _bridgeGameDocuments.GetById(id);
            var user = _users.GetById(UserId);
            var model = new ReplayViewModel(doc, user);
            return new JsonNetResult(model);
        }

        [GET("count")]
        public ActionResult Count()
        {
            return Content(_gamesManager.GamesCount.ToString());
        }


        private static GameModeEnum ParseGameMode(string input)
        {
            if (input == null)
            {
                return GameModeEnum.SpecificDeal;
            }
            switch (input.ToLower())
            {
                case "random":
                    return GameModeEnum.RandomDeal;
                case "attack":
                    return GameModeEnum.PracticeAttackSets;
                case "defence":
                    return GameModeEnum.PracticeDefenceSets;
                case "auto":
                    return GameModeEnum.WatchRobotsPlay;
                case "tournament":
                    return GameModeEnum.Tournament;
                case "repetition":
                    return GameModeEnum.Repetition;
                default:
                    throw new ArgumentOutOfRangeException(input);
            }
        }
    }
}