using PAQK.Domain.Aggregates.Game.Commands;
using PAQK.Platform.Dispatching.Interfaces;
using PAQK.Platform.Domain.Interfaces;

namespace PAQK.Domain.Aggregates.Game
{
    public class GameApplicationService : IMessageHandler
    {
        private readonly IRepository<GameAggregate> _repository;

        public GameApplicationService(IRepository<GameAggregate> repository)
        {
            _repository = repository;
        }

        public void Handle(CreateGame c)
        {
            _repository.Perform(c.Id,game => game.Create(c.Id, c.Users));
        }
    }
}