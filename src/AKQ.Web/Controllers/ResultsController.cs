using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AKQ.Domain.Documents;
using AKQ.Domain.Documents.Progress;
using AKQ.Domain.Services;
using AKQ.Domain.Utils;
using AKQ.Web.Controllers.ActionResults;
using AKQ.Web.Models;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace AKQ.Web.Controllers
{
    [RoutePrefix("results")]
    public class ResultsController : BaseController
    {
        private readonly UsersService _users;
        private readonly UserProgressService _progressService;

        public ResultsController(UsersService users, UserProgressService progressService)
        {
            _users = users;
            _progressService = progressService;
        }

        [POST("repetition")]
        public ActionResult Repetition(string userId)
        {
            var doc = _progressService.GetById(userId ?? UserId);
            var user = _users.GetById(userId ?? UserId);
            var model = new RepetitionResultsModel(doc.RepetitionProgress.Deals, user);
            return new JsonNetResult(model);
        }
    }

    public class RepetitionResultsModel
    {
        public RepetitionResultsModel(IEnumerable<DealStats> deals, User user)
        {
            Items = deals.Batch(5).Select(x => new DealsSetViewModel(x.ToList(), user));
        }

        public IEnumerable<DealsSetViewModel> Items { get; set; }
    }
}
