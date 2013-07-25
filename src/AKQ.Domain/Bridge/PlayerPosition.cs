using System;
using System.Collections.Generic;
using System.Linq;

namespace AKQ.Domain
{
    [Serializable]
    public struct PlayerPosition
    {
        private readonly string _shortName;
        private readonly string _fullName;
        private readonly int _index;

        private static readonly Dictionary<string, PlayerPosition> ShortNameMapping = new Dictionary<string, PlayerPosition>();
        private static readonly Dictionary<PlayerPosition, PlayerPosition> NextPlayers;

        public static readonly PlayerPosition West;
        public static readonly PlayerPosition East;
        public static readonly PlayerPosition South;
        public static readonly PlayerPosition North;

        private PlayerPosition(string shortName, string fullName, int index)
        {
            _shortName = shortName;
            _fullName = fullName;
            _index = index;
            ShortNameMapping[_shortName] = this;
        }

        static PlayerPosition()
        {
            West = new PlayerPosition("W","West", 0);
            North = new PlayerPosition("N", "North", 1);
            East = new PlayerPosition("E", "East",2 );
            South = new PlayerPosition("S", "South",3);
            NextPlayers = new Dictionary<PlayerPosition, PlayerPosition>
            {
                {West, North},
                {North, East},
                {East, South},
                {South, West},
            };
        }

        public string ShortName
        {
            get { return _shortName; }
        }

        public string FullName
        {
            get { return _fullName; }
        }

        public static PlayerPosition FromShortName(string s)
        {
            return ShortNameMapping[s.ToUpper()];
        }

        public string ToShortName()
        {
            return _shortName;
        }

        public static bool operator ==(PlayerPosition x, PlayerPosition y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(PlayerPosition x, PlayerPosition y)
        {
            return !x.Equals(y);
        }

        public PlayerPosition GetNextPlayerPosition()
        {
            return NextPlayers[this];
        }

        public PlayerPosition GetOppositePosition()
        {
            return GetNextPlayerPosition().GetNextPlayerPosition();
        }

        public bool IsNorthSouthTeam
        {
            get { return (this == South || this == North); }
        }

        public bool IsEastWestTeam
        {
            get { return (this == East || this == West); }
        }

        public int Index
        {
            get { return _index; }
        }

        public override string ToString()
        {
            return _fullName;
        }

        public bool IsInTeamWith(PlayerPosition declarer)
        {
            return this == declarer || this == declarer.GetOppositePosition();
        }
    }
}