﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using PAQK;
using PAQK.ViewServices;

namespace AKQ.Web.Controllers
{
    [Authorize]
    [RoutePrefix("tables")]
    public class TablesController : BaseController
    {
        private readonly TableViewService _tables;

        public TablesController(TableViewService tables)
        {
            _tables = tables;
        }


        [GET("")]
        public ActionResult Index()
        {
            return PartialView("Tables");
        }

        [POST("load")]
        public ActionResult GetAll()
        {
            var model = _tables.GetAll();
            return Json(model);
        }
    }
}