using System.Linq;
using System.Web.Mvc;
using Poker.Domain.Aggregates.Game.Commands;
using Poker.Platform.Mongo;
using Poker.Platform.Mvc;
using Poker.ViewServices;

namespace Poker.Web.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IdGenerator _idGenerator;
        private readonly TableViewService _tables;

        public AdminController(IdGenerator idGenerator, TableViewService tables)
        {
            _idGenerator = idGenerator;
            _tables = tables;
        }

        public ActionResult GenerateTables()
        {
            var all = _tables.GetAll();
            if (!all.Any())
            {
                for (int i = 0; i < 10; i++)
                {
                    var cmd = new CreateTable
                    {
                        Id = _idGenerator.Generate(),
                        BuyIn = 1000 + (i*100),
                        SmallBlind = 5 + (((i+1)/5)*10),
                        Name = "Table #" + (i + 1)
                    };
                    Send(cmd);
                }
                return Content("success!");
            }
            return Content("already created");
        }

        public ActionResult RegenerateTables()
        {
            var all = _tables.GetAll().ToList();
            foreach (var tableView in all)
            {
                Send(new ArchiveTable {Id = tableView.Id});
            }
            for (int i = 0; i < 10; i++)
            {
                var cmd = new CreateTable
                {
                    Id = _idGenerator.Generate(),
                    BuyIn = 1000 + (i*100),
                    SmallBlind = 5 + (((i +1)/5)*10),
                    Name = "Table #" + (i + 1)
                };
                Send(cmd);
            }
            return Content("success!");
        }
    }
}