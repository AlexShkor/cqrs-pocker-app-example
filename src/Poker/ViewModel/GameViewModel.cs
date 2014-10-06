using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Poker.Views;

namespace Poker.ViewModel
{
    public class GameViewModel
    {
        public GameViewModel(TableView view, string userId)
        {
            Id = view.Id;
            Name = view.Name;
            BuyIn = view.BuyIn;
            SmallBlind = view.SmallBlind;
            Bank = view.Bank;
            MyId = userId;
            CurrentPlayerId = view.CurrentPlayerId;
            Deck = view.Deck.Select(x => new CardViewModel(x)).ToList();
            Players = view.Players.OrderBy(x => x.Position).Select(x => new PlayerViewModel(x, userId)).ToList();
            MaxBid = view.Players.Select(x => x.Bid).Max();
            IsGuest = view.Players.All(x => x.UserId != userId);
            MinBet = view.MinBet;
        }

        public long MinBet { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string CurrentPlayerId { get; set; }

        public string MyId { get; set; }

        public long BuyIn { get; set; }

        public long SmallBlind { get; set; }

        public long Bank { get; set; }

        public List<CardViewModel> Deck { get; set; }

        public List<PlayerViewModel> Players { get; set; }

        public long MaxBid { get; set; }

        public bool IsGuest { get; set; }
    }
}