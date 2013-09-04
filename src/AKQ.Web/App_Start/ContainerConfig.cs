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

using System.Web.Routing;
using Microsoft.AspNet.SignalR;
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