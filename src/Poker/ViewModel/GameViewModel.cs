﻿using System.Collections.Generic;
using System.Linq;
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
            Deck = view.Deck.Select(x => new CardViewModel(x)).ToList();
            Players = view.Players.OrderBy(x=> x.Position).Select(x => new PlayerViewModel(x, userId)).ToList();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public long BuyIn { get; set; }

        public long SmallBlind { get; set; }

        public List<CardViewModel> Deck { get; set; } 

        public List<PlayerViewModel> Players { get; set; } 
    }
}