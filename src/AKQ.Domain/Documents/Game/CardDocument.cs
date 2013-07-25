namespace AKQ.Domain.Documents
{
    public class CardDocument
    {
        public string Suit { get; set; }
        public string Value { get; set; }

        public CardDocument()
        {
            
        }

        public CardDocument(Card card)
        {
            Suit = card.Suit.ShortName;
            Value = card.Rank.ShortName;
        }

        public Card ToDomainObject()
        {
            return new Card(Domain.Suit.FromShortName(Suit), Rank.FromString(Value));
        }
    }
}