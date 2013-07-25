using System.Collections.Generic;
using System.Linq;

namespace AKQ.Domain.ViewModel
{
    public class HandViewModel
    {
        public bool IsVisible { get; set; }
        public bool HasControl { get; set; }

        public List<List<CardViewModel>> Cards { get; set; }

        public string PlayerName { get; set; }

        public HandViewModel(){}

        public HandViewModel(Hand hand, bool visible, bool enableCards, Suit led, Suit trump)
        {
            if (visible)
            {
                IsVisible = true;
                var enableAll = !(hand.HasSuitCard(led) || hand.HasSuitCard(trump));
                Cards = new List<List<CardViewModel>>();
                MapCards(hand.GetCards(Suit.Spades), Suit.Spades, enableCards, led, trump, enableAll);
                MapCards(hand.GetCards(Suit.Hearts), Suit.Hearts, enableCards, led, trump, enableAll);
                MapCards(hand.GetCards(Suit.Clubs), Suit.Clubs, enableCards, led, trump, enableAll);
                MapCards(hand.GetCards(Suit.Diamonds), Suit.Diamonds, enableCards, led, trump, enableAll);
            }
            else
            {
                Cards = Enumerable.Range(0, 4).Select(x => new List<CardViewModel>()).ToList();
            }
        }

        private void MapCards(IEnumerable<Card> cards, Suit suit, bool enableCards, Suit led, Suit trump, bool enableAll)
        {
            var selected = enableCards && (enableAll || led == suit || trump == suit);
            MapCards(cards, suit, selected);
        }

        private void MapCards(IEnumerable<Card> cards, Suit suit, bool enabled)
        {
            var suitCards = cards.Where(x => x.Suit == suit).OrderByDescending(x => x.ScoreValue).Select(card => new CardViewModel(card, enabled)).ToList();
            Cards.Add(suitCards);
        }
    }
}