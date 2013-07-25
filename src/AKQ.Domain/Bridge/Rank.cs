using System;
using System.Collections.Generic;
using System.Linq;
using AKQ.Domain.Utils;

namespace AKQ.Domain
{
    [Serializable]
    public struct Rank : IComparable<Rank>
    {
        public bool Equals(Rank other)
        {
            return _score == other._score && string.Equals(_fullName, other._fullName) && string.Equals(_shortName, other._shortName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Rank && Equals((Rank) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return _score;
            }
        }

        private readonly string _shortName;
        public string ShortName { get { return _shortName; } }
        private readonly int _score;
        public int Score { get { return _score; } }
        private readonly string _fullName;
        public string FullName { get { return _fullName; } }

        public static readonly Rank Two;
        public static readonly Rank Three;
        public static readonly Rank Four;
        public static readonly Rank Five;
        public static readonly Rank Six;
        public static readonly Rank Seven;
        public static readonly Rank Eight;
        public static readonly Rank Nine;
        public static readonly Rank Ten;
        public static readonly Rank Jack;
        public static readonly Rank Queen;
        public static readonly Rank King;
        public static readonly Rank Ace;

        static Rank()
        {
            Two = new Rank(2, "2", "Two");
            Three = new Rank(3, "3", "Three");
            Four = new Rank(4, "4", "Four");
            Five = new Rank(5, "5", "Five");
            Six = new Rank(6, "6", "Six");
            Seven = new Rank(7, "7", "Seven");
            Eight = new Rank(8, "8", "Eight");
            Nine = new Rank(9, "9", "Nine");
            Ten = new Rank(10, "10", "Ten", new[] {"T"});
            Jack = new Rank(11, "J", "Jack");
            Queen = new Rank(12, "Q", "Queen");
            King = new Rank(13, "K", "King");
            Ace = new Rank(14, "A", "Ace");
        }

        private static readonly Dictionary<string, Rank> Ranks = new Dictionary<string, Rank>();
        private static readonly SortedSet<Rank> AllRanks = new SortedSet<Rank>(); 

        private Rank(int score, string shortName, string fullName, IEnumerable<string> additionalMappingValues = null)
        {
            Guard.Against(score < 2 && score > 14, "Score should be in range of 2 - 14");
            _score = score;
            _shortName = shortName;
            _fullName = fullName;
            Ranks[shortName] = this;
            AllRanks.Add(this);
            if (additionalMappingValues != null)
            {
                foreach (var mappingValue in additionalMappingValues)
                {
                    Ranks[mappingValue] = this;
                }
            }
        }

        public static IEnumerable<Rank> GetAll()
        {
            return AllRanks;
        }

        public static Rank FromString(string value)
        {
            Guard.Against(!Ranks.ContainsKey(value), "invalid mapping value for card rank.");
            return Ranks[value];
        }

        public static Rank FromChar(char value)
        {
            return FromString(value.ToString());
        }

        public int CompareTo(Rank other)
        {
            return Score.CompareTo(other.Score);
        }

        public static bool operator >(Rank x, Rank y)
        {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <(Rank x, Rank y)
        {
            return x.CompareTo(y) < 0;
        }

        public static bool operator >=(Rank x, Rank y)
        {
            return x.CompareTo(y) >= 0;
        }

        public static bool operator <=(Rank x, Rank y)
        {
            return x.CompareTo(y) <= 0;
        }

        public static bool operator ==(Rank x, Rank y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Rank x, Rank y)
        {
            return !x.Equals(y);
        }

        public static Rank NextRank(Rank rank)
        {
            if (rank.Score == 14)
            {
                return rank;
            }
            return Ranks.Values.First(x => x.Score == rank.Score + 1);
        }
    }
}