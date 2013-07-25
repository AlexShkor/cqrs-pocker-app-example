// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructuremapMvc.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AKQ.Domain;
using AKQ.Domain.EventHandlers;
using AKQ.Domain.Infrastructure;
using AKQ.Domain.Messaging;
using AKQ.Domain.Services;
using AKQ.Web.App_Start.DependencyResolution;
using AKQ.Web.Controllers;
using AKQ.Web.Serialization;
using Facebook;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Json;
using Microsoft.Practices.ServiceLocation;
using NLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PAQK.Platform.StructureMap;
using StructureMap;

namespace AKQ.Web.App_Start
{
    public static class ContainerConfig
    {
        public static void Configure()
        {
            IContainer container = ObjectFactory.Container;
            new PAQK.Bootstrapper().Configure(container);
            GlobalHost.DependencyResolver = new SignalrStructureMapResolver(container);
            RouteTable.Routes.MapHubs(new HubConfiguration() { Resolver = GlobalHost.DependencyResolver });
        }
    }
}