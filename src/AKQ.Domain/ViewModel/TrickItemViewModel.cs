using AKQ.Domain.Services;

namespace AKQ.Domain.ViewModel
{
    public class TrickItemViewModel
    {
        public CardViewModel TrickCard { get; set; }

        public bool IsCurrent { get; set; }

        public bool IsEmpty { get; set; }

        public TrickItemViewModel(Card? card)
        {
            if (card != null)
            {
                TrickCard = new CardViewModel(card.Value, false);
            }
            else
            {
                IsEmpty = true;
            }
        }
    }
}