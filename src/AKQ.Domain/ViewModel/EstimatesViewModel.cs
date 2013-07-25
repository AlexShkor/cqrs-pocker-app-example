using System;
using System.Collections.Generic;
using System.Linq;

namespace AKQ.Domain.ViewModel
{
    public class EstimatesViewModel
    {
        public List<SuitEstimateViewModel> Suits { get; set; }

        public EstimatesViewModel(Dictionary<PlayerPosition, Hand> originalHands, PlayerPosition editable, PlayerPosition estimated)
        {
            var suits = new Dictionary<Suit, SuitEstimateViewModel>();
            Action<Suit> initSuit = (suit) => suits.Add(suit, new SuitEstimateViewModel(suit, editable, estimated));
            initSuit(Suit.Spades);
            initSuit(Suit.Hearts);
            initSuit(Suit.Diamonds);
            initSuit(Suit.Clubs);
            foreach (var hand in originalHands)
            {
                if (hand.Key != editable && hand.Key != estimated)
                {
                    foreach (var card in hand.Value.GetAll())
                    {
                        suits[card.Suit][hand.Key].Inc();
                    }
                }
            }
            Suits = suits.Values.ToList();
        }
    }
}