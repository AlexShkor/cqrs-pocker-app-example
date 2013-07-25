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
    }
}