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

        [HttpPost]
        public JsonNetResult Upload(HttpPostedFileBase file)
        {
            var deals = new List<BridgeDeal>();

            using (var reader = new StreamReader(file.InputStream))
            {
                string line;
                List<string> list = null;
                var active = false;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("B"))
                    {
                        list = new List<string>();
                        active = true;
                    }
                    if (active)
                        list.Add(line);

                    if (line.StartsWith("M"))
                    {
                        active = false;
                        var deal = new BridgeDeal(list);
                        deal.Id = _idGenerator.Generate();
                        deals.Add(deal);
                    }
                }
            }

            _bridgeDealService.InsertBatch(deals.ToArray());

            return new JsonNetResult("done");
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

        //public ActionResult SendUsersToSegmentIo()
        //{
        //    var users = _usersService.GetAll();
        //    foreach (var user in users)
        //    {
        //        Analytics.Client.Identify(user.Id, new Traits()
        //        {
        //            {"Name", user.Username},
        //            {"Email", user.Email},
        //            {"Facebook Id", user.FacebookId}
        //        }, user.Registred);
        //    }
        //    return Content("success");
        //}
    }
}