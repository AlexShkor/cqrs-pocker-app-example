﻿using System.Collections.Generic;
using System.Web.UI.WebControls;
using Poker.Databases;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Domain.Data;
using Poker.Platform.Dispatching;
using Poker.Platform.Dispatching.Attributes;
using Poker.Platform.Dispatching.Interfaces;
using Poker.Views;
using Uniform;

namespace Poker.Handlers.ViewHandlers
{
    [Priority(PriorityStages.ViewHandling)]
    public class TableViewHandler : IMessageHandler
    {
        private readonly ViewDatabase _db;
        private readonly IDocumentCollection<TableView> _tables;

        public TableViewHandler(ViewDatabase db)
        {
            _db = db;
            _tables = db.Tables;
        }

        public void Handle(TableCreated e)
        {
            _tables.Save(new TableView()
            {
                Id = e.Id,
                Name = e.Name,
                BuyIn = e.BuyIn,
                SmallBlind = e.SmallBlind,
                MaxPlayers = e.MaxPlayers
            });
        }

        public void Handle(CardsDealed e)
        {
            _tables.Update(e.Id, view =>
            {
                foreach (var playerCard in e.Cards)
                {
                    view.AddPlayerCard(playerCard.UserId, playerCard.Card);
                }
            });
        }

        public void Handle(TableArchived e)
        {
            _tables.Delete(e.Id);
        }

        public void Handle(GameFinished e)
        {
            _tables.Update(e.Id, view =>
            {
                view.Deck = new List<Card>();
                foreach (var player in view.Players)
                {
                    player.Cards = new List<Card>();
                    player.Bid = 0;
                    player.IsSmallBlind = false;
                    player.IsBigBlind = false;
                }
                view.Bank = 0;
            });
        }

        public void Handle(PlayerJoined e)
        {
            var user = _db.Users.GetById(e.UserId);
            _tables.Update(e.Id, table => table.Players.Add(new PlayerDocument()
            {
                UserId = e.UserId,
                Position = e.Position,
                Cash = e.Cash,
                Name = user.UserName,
                AvatarId = user.AvatarId
            }));
        }

        public void Handle(BidMade e)
        {
            _tables.Update(e.Id, table => table.SetBid(e.Bid.UserId, e.Bid));
        }

        public void Handle(BiddingFinished e)
        {
            _tables.Update(e.Id, table =>
            {
                table.Bank = e.Bank;
                foreach (var player in table.Players)
                {
                    player.Bet = 0;
                }
            });
        }

        public void Handle(PlayerLeft e)
        {
            _tables.Update(e.Id, table => table.Players.RemoveAll(x => x.UserId == e.UserId));
        }

        public void Handle(NextPlayerTurned e)
        {
            _tables.Update(e.Id, table =>
            {
                foreach (var playerDocument in table.Players)
                {
                    playerDocument.CurrentTurn = playerDocument.UserId == e.Player.UserId;
                }
                table.CurrentPlayerId = e.Player.UserId;
                table.MinBet = e.MinBet;
            });
        }

        public void Handle(DeckDealed e)
        {
            _tables.Update(e.Id, table => table.Deck.AddRange(e.Cards));
        }
    }
}