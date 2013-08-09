using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AKQ.Domain.Documents;
using AKQ.Domain.Infrastructure;
using AKQ.Domain.Services;
using AKQ.Web.Controllers.ActionResults;
using Segmentio;
using Segmentio.Model;

namespace AKQ.Web.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        private readonly IdGenerator _idGenerator;
        private readonly UsersService _usersService;
        private readonly BridgeDealService _bridgeDealService;
        private readonly UserLogsService _usersLogs;

        public AdminController(IdGenerator idGenerator, BridgeDealService bridgeDealService, UsersService usersService, UserLogsService usersLogs)
        {
            _idGenerator = idGenerator;
            _bridgeDealService = bridgeDealService;
            _usersService = usersService;
            _usersLogs = usersLogs;
        }

        public ViewResult Hands()
        {
            return View();
        }


        public ActionResult AllUsers()
        {
            var model = _usersService.GetAllSorted();
            return View(model);
        }

        public ActionResult Logins()
        {
            var model = _usersLogs.GetAll().OrderByDescending(x=> x.Date);
            return View(model);
        }
    }
}