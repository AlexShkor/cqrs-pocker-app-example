﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices.Hands
{
    public class RoyalFlush : BasePokerHand
    {
        public override string Name
        {
            get
            {
                return PokerNames.RoyalFlush;
            }
        }

        public override int Score
        {
            get
            {
                return (int)PokerScores.RoyalFlush;
            }
        }

        public override bool IsPresent()
        {
            var straightFlash = new StraightFlush();
            straightFlash.SetCards(Cards.ToList());
            var isPresent = straightFlash.IsPresent() && straightFlash.HandCards.Any(c => c.Rank == Rank.Ten) && straightFlash.HandCards.Any(c => c.Rank == Rank.Ace);
            return isPresent;
        }

        protected override int CompareWithSame(IPokerHand other)
        {
            return 0;
        }
    }
}