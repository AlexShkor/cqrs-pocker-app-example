﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Poker.Hubs;
using Poker.Platform.Mvc;

namespace Poker.Web.Controllers
{
    public class ChatController : BaseController
    {
        public ActionResult Send(string message)
        {
            UsersHub.CurrentContext.Clients.All.chatMessage(new
            {
                Content = message,
                Time = DateTime.Now.ToShortTimeString(),
                Name = UserName
            });
            return new ContentResult();
        }

    }
}