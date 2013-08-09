using PAQK.Domain.Aggregates.Game.Commands;
using PAQK.Platform.Dispatching.Interfaces;
using PAQK.Platform.Domain.Interfaces;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game
{
    public class GameApplicationService : IMessageHandler
    {
        private readonly IRepository<GameTableAggregate> _repository;

        public GameApplicationService(IRepository<GameTableAggregate> repository)
        {
            _repository = repository;
        }

        public void Handle(CreateTable c)
        {
            _repository.Perform(c.Id,game => game.CreateTable(c.Id, c.Name, c.BuyIn, c.SmallBlind));
        }

        public void Handle(CreateGame c)
        {
            _repository.Perform(c.Id,game => game.CreateGame(c.GameId));
        }

        public void Handle(JoinTable c)
        {
            _repository.Perform(c.Id,table => table.JoinTable(c.UserId, c.Position,c.Cash));
        }
    }

    public class JoinTable: Command
    {
        public int Position { get; set; }

        public string UserId { get; set; }

        public long Cash { get; set; }
    }
}