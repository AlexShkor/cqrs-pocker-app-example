using System.Linq;
using System.Web.UI.WebControls;
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
            var ordered = Cards.Select(x => x.Rank.Score).ToList();
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
                    if (countInOrder > 5)
                    {
                        HandCards.RemoveAt(0);
                    }
                }
                else if (score == previousScore)
                {
                    continue;
                }
                else
                {
                    if (countInOrder >= 5)
                    {
                        return true;
                    }
                    HandCards.Clear();
                    HandCards.Add(GetCard(score));
                    countInOrder = 1;
                }
                previousScore = score;
            }
            return countInOrder >= 5;
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