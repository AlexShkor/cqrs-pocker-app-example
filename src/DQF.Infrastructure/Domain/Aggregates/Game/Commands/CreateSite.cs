using System.Collections.Generic;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Commands
{
    public class CreateGame : Command
    {
        public List<string> Users { get; set; }
    }
}