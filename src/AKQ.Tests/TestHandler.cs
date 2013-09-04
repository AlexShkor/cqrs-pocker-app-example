using System.Collections.Generic;
using AKQ.Domain.Messaging;
using AKQ.Domain.Services;

namespace AKQ.Tests
{
    public class TestHandler : BridgeEventHandler
    {
        public List<GameEvent> Events = new List<GameEvent>();

        public TestHandler()
        {
            
        }
    }
}