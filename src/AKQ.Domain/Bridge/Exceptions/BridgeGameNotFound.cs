namespace AKQ.Domain.Exceptions
{
    public class BridgeGameNotFound : BridgeGameException
    {
        public BridgeGameNotFound(string gameId)
        {
            GameId = gameId;
        }

        public string GameId { get; private set; }
    }
}