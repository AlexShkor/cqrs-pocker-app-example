using System.Collections.Generic;
using System.Linq;
using Poker.Views;

namespace Poker.ViewModel
{
    public class PlayerViewModel
    {
        public int Position { get; set; }
        public long Cash { get; set; }
        public long Bid { get; set; }
        public long RaiseValue { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public bool IsMe { get; set; }
        public bool CurrentTurn { get; set; }
        public List<CardViewModel> Cards { get; set; }
        public bool IsInGame { get; set; }
        public bool IsSmallBlind { get; set; }
        public bool IsBigBlind { get; set; }
        public string AvatarUrl { get; set; }

        public string BlindText
        {
            get
            {
                if (IsSmallBlind)
                    return "Small Blind";
                if (IsBigBlind)
                    return "Big Blind";

                return string.Empty;
            }
        }

        
        public PlayerViewModel(PlayerDocument doc, string myUserId)
        {
            Position = doc.Position;
            Cash = doc.Cash;
            Bid = doc.Bet;
            UserId = doc.UserId;
            Name = doc.Name;
            IsMe = UserId == myUserId;
            CurrentTurn = doc.CurrentTurn;
            IsSmallBlind = doc.IsSmallBlind;
            IsBigBlind = doc.IsBigBlind;
            IsInGame = doc.Cards.Count != 0;
            AvatarUrl = AvatarsService.GetUrlById(doc.AvatarId);

            if (IsMe)
            {
                Cards = doc.Cards.Select(x => new CardViewModel(x)).ToList();
            }
            else
            {
                Cards = new List<CardViewModel>();
            }
        }
    }
}