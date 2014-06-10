using System.Linq;
using Poker.Domain.Data;

namespace Poker.Domain.ApplicationServices.Hands
{
    public class Straight : BasePokerHand
    {
        public override string Name
        {
            get
            {
                return PokerNames.Straight;
            }
        }

        public override int Score
        {
            get
            {
                return (int)PokerScores.Straight;
            }
        }

        public override bool IsPresent()
        {
            var ordered = Cards.Select(x=> x.Rank.Score).ToList();
            if (ordered.Contains(Rank.Ace.Score))
            {
                ordered.Add(1);
            }
            ordered = ordered.OrderBy(x => x).ToList();
            var countInOrder = 1;
            var previousScore = ordered[0];
            HandCards.Add(GetCard(previousScore));
            for (int i = 1; i < ordered.Count; i++)
            {
                var score = ordered[i];
                if (score == previousScore + 1)
                {
                    countInOrder += 1;
                    HandCards.Add(GetCard(score));
                }
                else if (score == previousScore)
                {
                    continue;
                }
                else
                {
                    if (countInOrder >= 5)
                    {
                        TrimHandCards();
                        return true;
                    }
                    HandCards.Clear();
                    HandCards.Add(GetCard(score));
                    countInOrder = 1;
                }
                previousScore = score;
            }
            if (countInOrder >= 5)
            {
                TrimHandCards();
                return true;
            }
            return false;
        }

        private void TrimHandCards()
        {
            var handCards = HandCards.OrderByDescending(x => x.Rank).Take(5).ToList();
            HandCards.Clear();
            HandCards.AddRange(handCards);
        }

        private Card GetCard(int score)
        {
            if (score == 1)
            {
                score = Rank.Ace.Score;
            }
            return Cards.First(x => x.Rank.Score == score);
        }

        protected override int CompareWithSame(IPokerHand other)
        {
            return CompareHandMaxRank(other);
        }
    }
}