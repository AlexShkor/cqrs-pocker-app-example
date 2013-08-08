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

        public Dictionary<int,string> Places { get; private set; }

        public object CurrentGame { get; private set; }

        public string TableId { get; set; }

        public Pack Pack { get; set; }

        public GameTableState()
        {
            On((TableCreated e) =>
            {
                TableId = e.Id;
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
            On((UserJoined e) => JoinedPlayers.Add(e.UserId, new TablePlayer()
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
            foreach (var player in players)
            {
                Places.Add(player.Position,player.UserId);
            }
        }

        public string GetNextDealer()
        {
            throw new System.NotImplementedException();
        }

        public bool IsTableFull()
        {
            return JoinedPlayers.Count >= 10;
        }

        public bool IsPositionTaken(int position)
        {
            return JoinedPlayers.Values.Any(x => x.Position == position);
        }
    }

    public class TablePlayer
    {
        public string UserId { get; set; }

        public long Cash { get; set; }

        public int Position { get; set; }
    }

    public class UserJoined: Event
    {
        public string GameId { get; set; }

        public int Position { get; set; }

        public string UserId { get; set; }

        public long Cash { get; set; }
    }
}