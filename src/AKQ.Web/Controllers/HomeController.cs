using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AKQ.Domain;
using AKQ.Domain.Documents;
using AKQ.Domain.Documents.Progress;
using AKQ.Domain.Services;
using AKQ.Domain.Utils;
using AKQ.Web.Controllers.ActionResults;
using AKQ.Web.Models;
using AttributeRouting;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Mvc;
using DdsContract;

namespace AKQ.Web.Controllers
{
    [RoutePrefix("home")]
    public class HomeController : BaseController
    {
        private readonly UsersService _users;
        private readonly UserProgressService _progressService;
        private readonly BridgeGameDocumentsService _games;
        private readonly TournamentDocumentsService _tournaments;

        public HomeController(BridgeGameDocumentsService games, TournamentDocumentsService tournaments, UsersService users, UserProgressService progressService)
        {
            _games = games;
            _tournaments = tournaments;
            _users = users;
            _progressService = progressService;
        }

        [GET("")]
        public ActionResult Index()
        {
            return View("Templates/Empty","_BaseLayout");
        }

        [POST("history")]
        public ActionResult Hisotry(string userId, string dealId)
        {
            var user = _users.GetById(userId.HasValueOr(UserId));
            var games = dealId.HasValue()
                            ? _games.GetByDealIdForUser(dealId, userId.HasValueOr(UserId))
                            : _games.GetLastWeek(userId.HasValueOr(UserId));
            var model = games.Select(x => new ExtendedGameResultItem(x, user));
            return new JsonNetResult(model);
        }

        [Authorize]
        [GET("results/tournament")]
        public ActionResult TournamentsResults()
        {
            var model = new TournamentResultsViewModel
            {
                Items = _tournaments.GetFinished(UserId).Select(x => new TournamentResultItem(x,UserId)).ToList()
            };
            return View(model);
        }

        [GET("add-notes/{gameId}")]
        public ActionResult AddNotes(string gameId)
        {
            var doc = _games.GetById(gameId);
            var model = new AddNotesViewModel(gameId, doc.Notes);
            return View(model);
        }

        [POST("add-notes/{gameId}")]
        public ActionResult AddNotes(AddNotesViewModel model)
        {
            var doc = _games.GetById(model.GameId);
            doc.Notes = model.Notes;
            _games.Save(doc);
            return RedirectToAction("Results");
        }

        [POST("game-sets")]
        public ActionResult GameSets(string userId)
        {
            var doc = _progressService.GetById(userId ?? UserId);
            var user = _users.GetById(userId ?? UserId);
            var gameSet = doc.PracticeProgress.GameSets;
            var model = new GameSetsModel
            {
                Items = gameSet.Select(x => new PracticeGameSetViewModel(x, user)).ToList()
            };
            return new JsonNetResult(model);
        }

        public ActionResult Explain(string id)
        {
            var game = _games.GetById(id);
            var client = new DdsApiClient();
            var result = client.SolveGame(game.PBN);
            var order = new List<string>() { "W", "N", "E", "S" };
            var isPlayer = new Func<string, bool>(pos => pos == "S" || pos == "N");
            var model = new List<ExplainedTrickViewModel>();
            for (int i = 0; i < game.Tricks.Count; i++)
            {
                var item = new ExplainedTrickViewModel(i+1);
                foreach (var pos in order)
                {
                    var trick = game.Tricks[i];
                    var scores = result.Tricks[i][pos];
                    var card = trick.Cards.Find(x => x.Player == pos);
                    var currentScore = scores.Find(x => Card.FromString(x.Suit, x.Rank).Equals(Card.FromString(card.Suit, card.Value)));
                    var maxScore = scores.Max(x => x.Score);
                    var play = new PlayViewModel()
                    {
                        Card = card.Suit + card.Value,
                        IsWon = trick.Winner == card.Player,
                        IsWrong = isPlayer(pos) && (currentScore == null || maxScore != currentScore.Score)
                    };
                    if (play.IsWrong)
                    {
                        var nextRank = Rank.NextRank(Rank.FromString(card.Value));
                        var suggestions = scores.Where(x => x.Score == maxScore && !(x.Suit == card.Suit && x.Rank == nextRank.ShortName)).Select(x => x.Suit + x.Rank).ToList();
                        if (suggestions.Count == 0)
                        {
                            play.IsWrong = false;
                        }
                        else
                        {
                            
                        play.Tip = "Suggestions: " + string.Join(", ", suggestions );
                        }
                    }
                    item.Players.Add(play);
                }
                model.Add(item);
            }
            return View(model);
        }

        public ActionResult Download(string id)
        {
            var game = _games.GetById(id);
            var stream = new MemoryStream();
            var sw = new StreamWriter(stream);
            sw.Write(game.PBN);
            sw.Flush();
            stream.Position = 0;
            return File(stream, "text", "akqbridge-" + game.Id + ".pbn");
        }

        public ActionResult Games()
        {
            var games = _games.GetNotStarted();
            var model = new GamesListViewModel
            {
                Items = games.Select(x=> new GameListItem(x)).ToList()
            };
            return View(model);
        }

        [Authorize]
        public ActionResult Tournaments()
        {
            var docs = _tournaments.GetActual().OrderByDescending(x => x.StartTime);
            var model = new TournamentsListViewModel
            {
                Items = docs.Select(x => new TournamentItemViewModel(x)).ToList()
            };
            return View(model);
        }
    }

    public class PracticeGameSetViewModel : DealsSetViewModel
    {
        public int Index { get; set; }

        public PracticeGameSetViewModel(GameSet gameSet, User user)
            : base(gameSet.Deals, user)
        {
            Index = gameSet.Index;
        }
    }

    public class DealsSetViewModel
    {
        public int Rating { get; set; }

        public List<DealResultItem> Deals { get; set; }

        public List<string> Results { get; set; }

        public DealsSetViewModel(List<DealStats> deals, User user)
        {
            Rating = (100 * deals.Count(x => x.HasWinWithFirstAttemp())) / 5;
            Results = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                var wons = 0;
                var games = 0;
                foreach (var deal in deals)
                {
                    if (deal.DealGameStats.Count > i)
                    {
                        games++;
                        if (deal.DealGameStats[i].IsWin())
                        {
                            wons++;
                        }
                    }
                }
                if (games != 5)
                {
                    break;
                }
                Results.Add(((100 * wons) / 5).ToString(CultureInfo.InvariantCulture) + "%");
            }
            Deals = deals.Select(x => new DealResultItem(x, user.GetTags(x.DealId))).ToList();
        }
    }

    public class GameSetsModel
    {
        public List<PracticeGameSetViewModel> Items { get; set; }
    }
}