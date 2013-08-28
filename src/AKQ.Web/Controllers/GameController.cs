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
using PAQK.Domain.Aggregates.Game;
using PAQK.Domain.Aggregates.Game.Commands;
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
        [GET("view/{gameId}")]
        public ActionResult Index(string gameId)
        {
            ViewBag.Title = "Game";
            ViewBag.UserName = UserName;
            return View("Templates/Game", (object)gameId);
        }


        [POST("view/{gameId}")]
        public ActionResult Load(string gameId)
        {
            ViewBag.Title = "Game";
            ViewBag.UserName = UserName;
            return View("Templates/Game", (object)gameId);
        }

        [POST("join")]
        public ActionResult Join(string tableId)
        {
            var cmd = new JoinTable
            {
                Id = tableId,
                Cash = 1000,
                UserId = UserId
            };
            Send(cmd);
            return Json(cmd);
        }

        [POST("force")]
        public ActionResult Force(string tableId)
        {
            var cmd = new JoinTable
            {
                Id = tableId,
                Cash = 1000,
                UserId = UserId
            };
            Send(cmd);
            return Json(cmd);
        }
    }
}