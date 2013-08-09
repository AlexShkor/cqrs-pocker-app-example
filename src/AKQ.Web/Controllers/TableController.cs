using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAQK.ViewServices;

namespace AKQ.Web.Controllers
{
    public class TableController : BaseController
    {
        private readonly TableViewService _tables;

        public TableController(TableViewService tables)
        {
            _tables = tables;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
