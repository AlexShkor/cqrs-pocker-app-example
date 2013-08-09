using System.Collections.Generic;
using System.Linq;
using AKQ.Domain;
using PAQK.Domain.Aggregates.Game.Data;
using PAQK.Domain.Aggregates.Game.Events;
using PAQK.Platform.Domain;
using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game
{
    public sealed class GameTableState: AggregateState
    {
        public Dictionary<string,TablePlayer> JoinedPlayers { get; private set; }

        public Dictionary<int,GamePlayer> Places { get; private set; }

        public string CurrentGame { get; private set; }

        public string TableId { get; set; }

        public long BuyIn { get; private set; }

        public long SmallBlind { get; private set; }

        public Pack Pack { get; set; }

        public readonly int MaxPlayers = 10;

        public GameTableState()
        {
            On((TableCreated e) =>
            {
                TableId = e.Id;
                SmallBlind = e.SmallBlind;
                BuyIn = e.BuyIn;
            }); 
            On((GameFinished e) =>
            {
                CurrentGame = null;
                MoveDealer();
            });
            On((GameCreated e) =>
            {
                CurrentGame = e.GameId;
                Pack = new Pack(e.Cards);
                SitPlayers(e.Players);
            });
            On((PlayerJoined e) => JoinedPlayers.Add(e.UserId, new TablePlayer()
            {
                Cash = e.Cash,
                Position = e.Position,
                UserId = e.UserId,
            }));
        }

        private void MoveDealer()
        {
            throw new System.NotImplementedException();
        }

        private void SitPlayers(List<TablePlayer> players)
        {
            Places.Clear();
            foreach (var player in players.OrderBy(x=> x.Position))
            {
                Places.Add(player.Position,new GamePlayer()
                {
                    Position = player.Position,
                    UserId = player.UserId
                });
            }
        }

        public string GetNextDealer()
        {
            throw new System.NotImplementedException();
        }

        public bool IsTableFull()
        {
            return JoinedPlayers.Count >= MaxPlayers;
        }

        public bool IsPositionTaken(int position)
        {
            return JoinedPlayers.Values.Any(x => x.Position == position);
        }

        public bool HasUser(string userId)
        {
            return JoinedPlayers.ContainsKey(userId);
        }
    }

    public class TablePlayer
    {
        public string UserId { get; set; }

        public long Cash { get; set; }

        public int Position { get; set; }
    }

    public class PlayerJoined: Event
    {

        public int Position { get; set; }

        public string UserId { get; set; }

        public long Cash { get; set; }
    }
}