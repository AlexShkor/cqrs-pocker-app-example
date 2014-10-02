using System.Collections.Generic;
using System.Linq;
using Poker.Databases;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Events;
using Poker.Hubs;
using Poker.Platform.Dispatching;
using Poker.Platform.Dispatching.Attributes;
using Poker.Platform.Dispatching.Interfaces;
using Poker.ViewModel;

namespace Poker.Handlers.SingleUseEventHandlers.SignalR
{
    [Priority(PriorityStages.ViewHandling_After)]
    public class GameHubEventHandler : IMessageHandler
    {
        private readonly ViewDatabase _db;

        public GameHubEventHandler(ViewDatabase db)
        {
            _db = db;
        }

        public void Handle(NextPlayerTurned e)
        {
            UsersHub.CurrentContext.Clients.Group(e.Id).playerTurnChanged(new
            {
                TableId = e.Id,
                CurrentPlayerId = e.Player.UserId,
            });
        }

        public void Handle(BidMade e)
        {
            var table = _db.Tables.GetById(e.Id);
            var maxBid = table.Players.Select(x => x.Bid).Max();

            UsersHub.CurrentContext.Clients.Group(e.Id).bidMade(new
            {
                UserId = e.Bid.UserId,
                NewCashValue = e.Bid.NewCashValue,
                Bid = e.Bid.Bid,
                Bet = e.Bid.Bet,
                Odds = e.Bid.Odds,
                LastBet = e.Bid.LastBet,
                BidType = ((BidTypeEnum)e.Bid.BidType).ToString(),
                MaxBid = maxBid,
                MaxBet = e.Bid.GetMaxBet()
            });
        }

        public void Handle(CardsDealed e)
        {
            var cards = e.Cards.Select(x => new
             {
                 Card = new CardViewModel(x.Card),
                 UserId = x.UserId
             });

            UsersHub.CurrentContext.Clients.Group(e.Id).cardsDealed(new
            {
                Cards = cards
            });
        }

        public void Handle(DeckDealed e)
        {
            var deck = e.Cards.Select(x => new CardViewModel(x)).ToList();

            UsersHub.CurrentContext.Clients.Group(e.Id).deckDealed(new
            {
                Deck = deck
            });
        }


        public void Handle(GameCreated e)
        {
            UsersHub.CurrentContext.Clients.Group(e.Id).gameCreated(new
            {
                e = e
            });
        }

        public void Handle(GameFinished e)
        {
            UsersHub.CurrentContext.Clients.Group(e.Id).gameFinished(new
            {
                Winners = e.Winners.Select(x => new WinnerViewModel(x))
            });

        }

        public void Handle(PlayerJoined e)
        {
            var table = _db.Tables.GetById(e.Id);
            var newPlayer = table.Players.Find(p => p.UserId == e.UserId);
            UsersHub.CurrentContext.Clients.Group(e.Id).playerJoined(new
            {
                NewPlayer = new PlayerViewModel(newPlayer, "")
            });

        }

        public void Handle(BiddingFinished e)
        {
            UsersHub.CurrentContext.Clients.Group(e.Id).biddingFinished(new
            {
                Bank = e.Bank
            });

        }
    }
}