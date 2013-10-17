using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.Game.Commands
{
    public class CreateGame : Command
    {
        public string GameId { get; set; }
    }
}