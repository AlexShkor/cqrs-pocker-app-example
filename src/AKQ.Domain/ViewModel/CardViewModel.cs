 

namespace AKQ.Domain.ViewModel
{
    public class CardViewModel
    {
        public string Value { get; set; }
        public string Suit { get; set; }
        public string Symbol { get; set; }
        public string Color { get; set; }
        public bool IsSelectable { get; set; }

        public CardViewModel()
        {
            
        }

        public CardViewModel(Card card, bool enabled)
        {
            Suit = card.Suit.ToShortName();
            Value = card.Value;
            Symbol = card.Suit.ToSymbol();
            Color = card.Suit.GetColor();
            IsSelectable = enabled;
        }
    }
}