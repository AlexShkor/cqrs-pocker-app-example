namespace AKQ.Domain
{
    public class CreateGame : GameCommand
    {
        public string UserName { get; set; }

        public string DealId { get; set; }

        public GameModeEnum GameMode { get; set; }

        public string TournamentId { get; set; }
    }
}