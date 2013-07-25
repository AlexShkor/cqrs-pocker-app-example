namespace AKQ.Domain.Documents
{
    public class PlayerCardDocument: CardDocument
    {
        public string Player { get; set; }

        public PlayerCardDocument(Card card, PlayerPosition pos):base(card)
        {
            Player = pos.ToShortName();
        }
    }
}