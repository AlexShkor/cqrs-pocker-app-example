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
using StructureMap;

namespace AKQ.Web.App_Start
{
    public static class ContainerConfig
    {
        public static void Configure()
        {
            ObjectFactory.Initialize(x => x.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            }));

            IContainer container = ObjectFactory.Container;
            var resolver = new StructureMapDependencyResolver(container);
            container.Configure(config =>
            {
                config.For<Logger>().Use(x => LogManager.GetCurrentClassLogger());
                config.For<IRoboBridgeAI>().Use<RoboBridgeAI>();
                config.For<GamesManager>().Singleton().Use<GamesManager>();
                config.For<IServiceLocator>().Singleton().Use(resolver);
                config.For<IBridgeGameCallback>().Use<BridgeGameCallback>();
                config.For<IEventHandler>().Use<UserInterfaceReactor>();
                config.For<IEventHandler>().Use<BridgeAICommandsProducer>();
                config.For<IEventHandler>().Use<BridgeGameFinilizer>();
                config.For<IEventHandler>().Use<BridgeGameDocumentHandler>();
                config.For<IEventHandler>().Use<TournamentDocumentHandler>();
                config.For<IEventHandler>().Use<UserProgressHandler>();
                config.For<IEventHandler>().Use<RepetitionUserProgressHandler>();
                config.For<IEventHandler>().Use<StatiscticsHandler>();
            });

            //Domain
            new Bootstaper().Configure(container);
            
            //MVC
            DependencyResolver.SetResolver(resolver);
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(container);
            //SignalR
            GlobalHost.DependencyResolver = new StructureMapResolver(container);
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = new JsonNetSerializer(settings);
            GlobalHost.DependencyResolver.Register(typeof(IJsonSerializer), () => serializer);
           // GlobalHost.DependencyResolver.UseRedis("pub-redis-19094.us-east-1-2.3.ec2.garantiadata.com", 19094, "SBSNZmFRzQ249Dt0", new[] { "signalr.key" });
            RouteTable.Routes.MapHubs(new HubConfiguration() { Resolver = GlobalHost.DependencyResolver });


            //Paralect platform
            new PAQK.Bootstrapper().Configure(container);
        }
    }
}