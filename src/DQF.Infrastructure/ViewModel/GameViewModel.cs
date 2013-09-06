using System.Collections.Generic;
using System.Linq;
using PAQK.Views;

namespace PAQK.ViewModel
{
    public class GameViewModel
    {
        public GameViewModel(TableView view, string userId)
        {
            Id = view.Id;
            Name = view.Name;
            BuyIn = view.BuyIn;
            SmallBlind = view.SmallBlind;
            MyCards = view.Players.Find(x => x.UserId == userId).Cards.Select(x => new CardViewModel(x)).ToList();
            Deck = view.Deck.Select(x => new CardViewModel(x)).ToList();
            Players = view.Players.OrderBy(x=> x.Position).Select(x => new PlayerViewModel(x)).ToList();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public long BuyIn { get; set; }

        public long SmallBlind { get; set; }

        public List<CardViewModel> Deck { get; set; } 

        public List<CardViewModel> MyCards { get; set; } 

        public List<PlayerViewModel> Players { get; set; } 
    }

    public class PlayerViewModel
    {
        public int Position { get; set; }
        public long Cash { get; set; }
        public long Bid { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }

        public PlayerViewModel(PlayerDocument doc)
        {
            Position = doc.Position;
            Cash = doc.Cash;
            Bid = doc.Bid;
            UserId = doc.UserId;
            Name = doc.Name;
        }
    }
}