using PAQK.Domain.Aggregates.Game.Commands;
using PAQK.Platform.Dispatching.Interfaces;
using PAQK.Platform.Domain.Interfaces;

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
            _repository.Perform(c.Id,game => game.CreateTable(c.Id));
        }

        public void Handle(CreateGame c)
        {
            _repository.Perform(c.Id,game => game.CreateGame(c.GameId));
        }
    }
}