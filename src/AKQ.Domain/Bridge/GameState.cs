namespace AKQ.Domain
{
    public class GameState
    {
        public string Id { get; private set; }

        public Trick CurrentTrick { get; private set; }

        public bool Started { get; private set; }

        public bool Finished { get { return CompletedTricks == 13; } }

        public int CompletedTricks { get; private set; }

        public Deck Hands { get; private set; }

        public string DealId { get; private set; }

        protected string HostUserId { get; private set; }

        public Contract Contract { get; private set; }
    }
}