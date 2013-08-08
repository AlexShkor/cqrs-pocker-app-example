using System.Collections.Generic;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Commands
{
    public class CreateGame : Command
    {
        public string GameId { get; set; }
    }

    public class CreateTable : Command
    {
    }
}