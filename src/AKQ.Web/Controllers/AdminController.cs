using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PAQK;
using PAQK.Domain.Aggregates.Game.Commands;
using PAQK.Infrastructure;
using PAQK.ViewServices;

namespace AKQ.Web.Controllers
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
                        SmallBlind = 5 + ((i/5)*10),
                        Name = "Table #" + (i + 1)
                    };
                    Send(cmd);
                }
                return Content("success!");
            }
            return Content("already created");
        } 
    }
}