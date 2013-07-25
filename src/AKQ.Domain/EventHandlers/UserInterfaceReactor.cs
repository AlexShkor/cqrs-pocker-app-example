using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AKQ.Domain.Hubs;
using AKQ.Domain.Messaging;
using AKQ.Domain.Services;
using AKQ.Domain.UserEvents;
using AKQ.Domain.ViewModel;
using Microsoft.AspNet.SignalR;

namespace AKQ.Domain.EventHandlers
{
    [ProcessingOrder(QueueNamesEnum.UI, Order = 0)]
    public sealed class UserInterfaceReactor : BridgeEventHandler
    {
        private static IHubContext HubContext
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<BridgeHub>();
            }
        }

        public UserInterfaceReactor()
        {
            AddGameSignal<CardPlayed>((e, game) => game.cardPlayed(new
            {
                Card = new CardViewModel(e.Card, false),
                Player = e.Player.ToShortName()
            }));
            AddGameSignal<PlayerTurn>((e, game) =>
            {
                if (e.IsNextTrick)
                {
                    Thread.Sleep(1000);
                }
                game.playerTurnStarted(new
                {
                    Player = e.Player.ToShortName(),
                    LedSuit = e.LedSuit.ToShortName()
                });
            });
            AddGameSignal<TrickEnded>((e, game) => game.trickEnded(new
            {
                Winner = e.Winner.ToShortName()
            }));
            AddGameSignal<GameFinished>((e, game) =>
            {
                var suitsCount = new Dictionary<string, Dictionary<string, int>>();
                var addSuitCount = new Action<Suit, PlayerPosition>((suit, pos) =>
                {
                    var suitShortName = suit.ToShortName();
                    var posShortName = pos.ToShortName();
                    if (!suitsCount.ContainsKey(suitShortName))
                    {
                        suitsCount[suitShortName] = new Dictionary<string, int>();
                    }
                    suitsCount[suitShortName][posShortName] = e.OriginalHands[pos].GetCards(suit).Count;
                });
                foreach (var pos in e.OriginalHands.Keys)
                {
                    addSuitCount(Suit.Spades, pos);
                    addSuitCount(Suit.Hearts, pos);
                    addSuitCount(Suit.Diamonds, pos);
                    addSuitCount(Suit.Clubs, pos);
                }
                var originalHands = e.OriginalHands.ToDictionary(k => k.Key.ShortName, v => new HandViewModel(v.Value, true, false, Suit.NoTrumps, Suit.NoTrumps));

                var message = new
                {
                    OriginalHands = originalHands,
                    SuitsCount = suitsCount
                };
                game.gameFinished(message);
            });
            AddGameSignal<PlayerJoined>((e, game) => game.playerJoined(new
            {
                Position = e.Position.ToShortName(),
                UserId = e.UserId,
                Name = e.Name
            }));
            AddGameSignal<GameStarted>((e, game) => game.gameStarted(new { }));
        }

        private void AddGameSignal<TMessage>(Action<TMessage,dynamic> perform)where TMessage:GameEvent
        {
            AddHandler<TMessage>(e =>
            {
                var group = HubContext.Clients.Group(e.GameId);
                perform(e, group);
            });
        }
    }
}