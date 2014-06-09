using Poker.Domain.Aggregates.Game.Commands;
using Poker.Platform.Dispatching.Interfaces;
using Poker.Platform.Domain.Interfaces;

namespace Poker.Domain.Aggregates.Game
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

        public void Handle(ArchiveTable c)
        {
            _repository.Perform(c.Id,game => game.Archive());
        }

        public void Handle(JoinTable c)
        {
            _repository.Perform(c.Id,table => table.JoinTable(c.UserId,c.Cash));
        }

        #region Bidding

        public void Handle(CallBid c)
        {
            _repository.Perform(c.Id,table => table.Call(c.UserId));
        }

        public void Handle(CheckBid c)
        {
            _repository.Perform(c.Id,table => table.Check(c.UserId));
        }

        public void Handle(RaiseBid c)
        {
            _repository.Perform(c.Id,table => table.Raise(c.UserId, c.Amount));
        }

        public void Handle(FoldBid c)
        {
            _repository.Perform(c.Id,table => table.Fold(c.UserId));
        }

        #endregion
    }
}