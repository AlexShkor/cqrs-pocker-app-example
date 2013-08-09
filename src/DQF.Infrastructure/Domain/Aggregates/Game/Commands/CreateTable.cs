using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Commands
{
    public class CreateTable : Command
    {
        public string Name { get; set; }

        public long BuyIn { get; set; }

        public long SmallBlind { get; set; }
    }
}