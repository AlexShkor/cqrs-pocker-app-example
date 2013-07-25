namespace AKQ.Domain
{
    public class PlayCard : GameCommand
    {
        public Card Card { get; set; }

        public PlayerPosition Player { get; set; }
    }
}