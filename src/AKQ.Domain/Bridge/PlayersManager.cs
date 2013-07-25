using System;
using System.Collections.Generic;
using System.Linq;

namespace AKQ.Domain
{
    [Serializable]
    public class PlayersManager
    {
        protected readonly Dictionary<PlayerPosition, Player> Positions = new Dictionary<PlayerPosition, Player>();

        public Player West
        {
            get { return Positions[PlayerPosition.West]; }
        }

        public Player North
        {
            get { return Positions[PlayerPosition.North]; }
        }

        public Player East
        {
            get { return Positions[PlayerPosition.East]; }
        }

        public Player South
        {
            get { return Positions[PlayerPosition.South]; }
        }

        public Player FirstPlayer { get; private set; }
        public Player CurrentPlayer { get; set; }
        public Player Declarer { get; private set; }
        public Player Dummy { get; private set; }

        public PlayerPosition CurrentPosition
        {
            get { return CurrentPlayer.PlayerPosition; }
        }

        public Player CurrentOrPartner
        {
            get { return CurrentPlayer.IsDummy ? Opposite(CurrentPlayer) : CurrentPlayer; }
        }

        public PlayersManager()
        {
            SetPlayer(new Player(PlayerPosition.West, "Sally"));
            SetPlayer(new Player(PlayerPosition.North, "John"));
            SetPlayer(new Player(PlayerPosition.East, "Steve"));
            SetPlayer(new Player(PlayerPosition.South, "Mike"));
        }

        private void SetPlayer(Player player)
        {
            Positions[player.PlayerPosition] = player;
        }

        public IEnumerable<Player> GetAll()
        {
            return Positions.Values;
        }

        public Player Get(string symbol)
        {
            var key = PlayerPosition.FromShortName(symbol);
            if (Positions.ContainsKey(key))
            {
                return Positions[key];
            }
            return null;
        }

        private Player GetNextPlayer(Player player)
        {
            return Positions[player.PlayerPosition.GetNextPlayerPosition()];
        }

        private Player Opposite(Player player)
        {
            return Positions[player.PlayerPosition.GetOppositePosition()];
            ;
        }

        public Player Get(PlayerPosition position)
        {
            return Positions[position];
        }

        public IEnumerable<Player> GetHumanPlayers()
        {
            return Positions.Values.Where(x => !string.IsNullOrEmpty(x.UserId));
        }

        public IEnumerable<PlayerPosition> NotVisiblePositionsFor(string currentUserId)
        {
            return Positions.Where(x => !(x.Value.HasControl(currentUserId) || x.Value.IsDummy)).Select(x => x.Key);
        }

        public void SetFirstPlayer(PlayerPosition dealer)
        {
            FirstPlayer = CurrentPlayer = Get(dealer);
        }

        public void NextPlayer()
        {
            CurrentPlayer = GetNextPlayer(CurrentPlayer);
        }

        public void SetDeclarer(PlayerPosition declarer)
        {
            Declarer = Get(declarer);
            Dummy = Opposite(Declarer);
            Dummy.IsDummy = true;
            Dummy.TakeControl(Declarer.ControledByUserId);
            CurrentPlayer = GetNextPlayer(Declarer);
        }

        public void TakePlace(PlayerPosition position, string userId, string username)
        {
            var place = Get(position);
            if (!place.IsAI)
            {
                return;
            }
            place.TakePlace(userId, username);
            var partner = Opposite(place);
            if (partner.IsDummy)
            {
                partner.TakeControl(userId);
            }
        }

        public bool CanJoin(string userId)
        {
            return Positions.Values.All(x => x.UserId != userId);
        }

        //public static PlayerPosition GetDealer(PlayerPosition originalDealer)
        //{
        //    var dealer = originalDealer;
        //    if (dealer == PlayerPosition.North)
        //    {
        //        dealer = PlayerPosition.South;
        //    }
        //    if (dealer == PlayerPosition.West)
        //    {
        //        dealer = PlayerPosition.East;
        //    }
        //    return dealer;
        //}
}
}