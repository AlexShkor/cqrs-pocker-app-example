 

using PAQK.Domain.Data;

namespace PAQK.ViewModel
{
    public class CardViewModel
    {
        public string Value { get; set; }
        public string Suit { get; set; }
        public string Symbol { get; set; }
        public string Color { get; set; }

        public CardViewModel()
        {
            
        }

        public CardViewModel(Card card)
        {
            Suit = card.Suit.ToShortName();
            Value = card.Value;
            Symbol = card.Suit.ToSymbol();
            Color = card.Suit.GetColor();
        }
    }
}