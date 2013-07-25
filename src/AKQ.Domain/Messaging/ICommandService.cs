using System.Threading.Tasks;

namespace AKQ.Domain.Messaging
{
    public interface ICommandService
    {
        void Send(GameCommand command);
    }

    public class CommandService : ICommandService
    {
        private readonly GamesManager _gamesManager;

        public CommandService(GamesManager gamesManager)
        {
            _gamesManager = gamesManager;
        }

        public void Send(GameCommand command)
        {
            Task.Factory.StartNew(() => ProcessCommand(command));
        }

        private void ProcessCommand(GameCommand command)
        {
            
        }
    }
}