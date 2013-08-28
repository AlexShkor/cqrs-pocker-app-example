﻿using System.Linq;
using MongoDB.Bson;
using PAQK.Databases;
using PAQK.Documents;
using PAQK.Domain.Aggregates.Game;
using PAQK.Domain.Aggregates.Game.Events;
using PAQK.Domain.Aggregates.User.Events;
using PAQK.Platform.Dispatching;
using PAQK.Platform.Dispatching.Attributes;
using PAQK.Platform.Dispatching.Interfaces;
using PAQK.Views;
using Uniform;
using TableView = PAQK.Views.TableView;

namespace PAQK.Handlers.ViewHandlers
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
                Id = ObjectId.GenerateNewId().ToString(),
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

        public void Handle(GameCreated e)
        {
            _tables.Update(e.Id, table => table.Players = e.Players.Select(x =>
            {
                var user = _db.Users.GetById(x.UserId);
                return new PlayerDocument()
                {
                    UserId = x.UserId,
                    Position = x.Position,
                    Cash = x.Cash,
                    Name = user.UserName
                };

            }).ToList());
        }

        //public void Handle(PlayerJoined e)
        //{
        //    var user = _db.Users.GetById(e.UserId);
        //    _tables.Update(e.Id, table => table.JoinedPlayers.Add(new PlayerDocument()
        //    {
        //        UserId = e.UserId,
        //        Position = e.Position,
        //        Cash = e.Cash,
        //        Name = user.UserName
        //    }));
        //}


        //public void Handle(PlayerLeft e)
        //{
        //    _tables.Update(e.Id, table => table.JoinedPlayers.RemoveAll(x => x.UserId == e.UserId));
        //}

    }
}