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

        private bool Check(int countInOrder)
        {
            if (countInOrder >= 5)
            {
                return true;
            }
            return false;
        }

        private Card GetCard(int score)
        {
            if (score == 1)
            {
                score = Rank.Ace.Score;
            }
            return Cards.First(x => x.Rank.Score == score);
        }


        // working 
        //public override bool IsPresent()
        //{
        //    var orderedDistinct = Cards.OrderBy(x => x.Rank).Select(x => x.Rank).Distinct().ToList();

        //    var hasAce = orderedDistinct.Any(r => r.Score == Rank.Ace.Score);

        //    var countInOrder = 1;
        //    var previousRank = orderedDistinct[0];
        //    for (int i = 1; i < orderedDistinct.Count; i++)
        //    {
        //        var rank = orderedDistinct[i];

        //        if (rank.Score == previousRank.Score + 1)
        //        {
        //            countInOrder += 1;
        //        }

        //        else if (countInOrder == 4 && hasAce)
        //        {
        //            countInOrder += 1;
        //        }

        //        else
        //        {
        //            countInOrder = 1;
        //        }

        //        if (countInOrder == 5)
        //        {
        //            return true;
        //        }

        //        previousRank = rank;
        //    }

        //    return false;
        //}


        //public override bool IsPresent()
        //{
        //    var orderedDistinct = Cards.OrderBy(x => x.Rank).GroupBy(c => c.Rank).Select(c => c.First()).ToList();
            
        //    var ace = orderedDistinct.Where(c => c.Rank.Score == Rank.Ace.Score);
        //    var hasAce = ace.Count() == 1;

        //    var countInOrder = 1;
        //    var previousCard = orderedDistinct[0];
        //    HandCards.Add(previousCard);

        //    for (int i = 1; i < orderedDistinct.Count; i++)
        //    {
        //        var card = orderedDistinct[i];

        //        if (card.Rank.Score == previousCard.Rank.Score + 1)
        //        {
        //            countInOrder += 1;
                    
        //            HandCards.Add(card);
        //        }

        //        else if (countInOrder == 4 && hasAce)
        //        {
        //            var hasTwo = HandCards.First().Rank.Score == Rank.Two.Score;
        //            var hasKing = HandCards.Last().Rank.Score == Rank.King.Score;

        //            if (hasTwo || hasKing)
        //            {
        //                countInOrder += 1;
        //                HandCards.Add(ace.Single());
        //            }

        //            else
        //            {
        //                countInOrder = 1;
        //                HandCards.Clear();
        //                HandCards.Add(card);
        //            }
        //        }

        //        else if (countInOrder < 5)
        //        {
        //            countInOrder = 1;
        //            HandCards.Clear();
        //            HandCards.Add(card);
        //        }

        //        previousCard = card;
        //    }





        //    return countInOrder >= 5;
        //}


        protected override int CompareWithSame(IPokerHand other)
        {
            return CompareHandMaxRank(other);
        }
    }
}