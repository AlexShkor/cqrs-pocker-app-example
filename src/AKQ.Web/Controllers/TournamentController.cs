using System;
using System.Linq;
using System.Web.Mvc;
using AKQ.Domain.Documents;
using AKQ.Domain.Services;
using AKQ.Web.Controllers.ActionResults;
using AKQ.Web.Models;

namespace AKQ.Web.Controllers
{
    public class TournamentController : BaseController
    {
        private readonly TournamentDocumentsService _tournaments;
        private readonly BridgeDealService _bridgeDealService;



        public TournamentController(TournamentDocumentsService tournaments, BridgeDealService bridgeDealService)
        {
            _tournaments = tournaments;
            _bridgeDealService = bridgeDealService;
        }

        [HttpPost]
        public ActionResult Load(bool orCreate)
        {
            var current = _tournaments.GetLastNotStarted();
            if (current == null)
            {
                if (orCreate)
                {
                    var deals =
                        Enumerable.Range(1, 3).Select(x => _bridgeDealService.GetRandomDeal(DealTypeEnum.Attack).Id).ToList();
                    current = new TournamentDocument(GenerateId(), UserId, deals);
                    _tournaments.Save(current);
                }
                else
                {
                    return new JsonNetResult("No tournaments found.");
                }
            }
            var model = new WaitTournamentViewModel
            {
                Registred = current.RegistredUsers.Contains(UserId),
                SecondsToStart = (int) (current.StartTime - DateTime.Now).TotalSeconds,
                TournamentId = current.Id,
                Players = current.RegistredUsers.Count
            };
            return new JsonNetResult(model);
        }

        [HttpPost]
        public ActionResult Register(string id)
        {
            var current = _tournaments.GetById(id);
            current.RegistredUsers.Add(UserId);
            _tournaments.Save(current);
            return new JsonNetResult(id);
        }

        [HttpPost]
        public ActionResult Withdraw(string id)
        {
            var current = _tournaments.GetById(id);
            current.RegistredUsers.Remove(UserId);
            _tournaments.Save(current);
            return new JsonNetResult(id);
        }
    }
}
