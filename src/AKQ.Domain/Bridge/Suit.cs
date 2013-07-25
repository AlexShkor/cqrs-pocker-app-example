using System;
using System.Collections.Generic;
using System.Linq;

namespace AKQ.Domain
{
    [Serializable]
    public struct Suit: IComparable<Suit>
    {
        private readonly int _value;
        private readonly string _shortName;
        private readonly string _fullName;
        private readonly string _html;
        private readonly string _htmlColor;

        private static readonly Dictionary<string, Suit> ShortNameToValue;
        private static readonly SortedSet<Suit> AllSuits = new SortedSet<Suit>(); 

        public static IEnumerable<Suit> GetAll()
        {
            return AllSuits;
        } 

        public static readonly Suit NoTrumps;
        public static readonly Suit Hearts;
        public static readonly Suit Diamonds;
        public static readonly Suit Clubs;
        public static readonly Suit Spades;

        private Suit(int value, string shortName, string fullName,  string html, string htmlColor)
        {
            _value = value;
            _shortName = shortName;
            _fullName = fullName;
            _html = html;
            _htmlColor = htmlColor;
        }

        static Suit()
        {
            ShortNameToValue = new Dictionary<string, Suit>();
            var addMapping = new Func<int,string, string, string, string,Suit>((int value, string shortName, string fullName, string html, string color) =>
            {
                var result = new Suit(value, shortName, fullName, html, color);
                ShortNameToValue.Add(shortName,result);
                AllSuits.Add(result);
                return result;
            });

            Clubs = addMapping(1,"C", "Clubs,", "&clubs;", "black");
            Diamonds = addMapping(2,"D", "Diamonds", "&diams;", "red");
            Hearts = addMapping(3,"H", "Hearts", "&hearts;", "red");
            Spades = addMapping(4,"S", "Spades", "&spades;", "black");
        }

        public string ShortName
        {
            get { return _shortName; }
        }

        public string FullName
        {
            get { return _fullName; }
        }

        public string Html
        {
            get { return _html; }
        }

        public string HtmlColor
        {
            get { return _htmlColor; }
        }

        public int Value
        {
            get { return _value; }
        }

        public string GetColor()
        {
            return _htmlColor;
        }

        public string ToShortName()
        {
            return _shortName;
        }

        public string ToSymbol()
        {
            return _html;
        }

        public int CompareTo(Suit other)
        {
            return Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return _shortName;
        }

        public static Suit FromShortName(string shortName)
        {
            return ShortNameToValue[shortName.ToUpper()];
        }

        public static bool operator ==(Suit x, Suit y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Suit x, Suit y)
        {
            return !x.Equals(y);
        }

        public static bool operator >(Suit x, Suit y)
        {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <(Suit x, Suit y)
        {
            return x.CompareTo(y) < 0;
        }

        public static bool operator >=(Suit x, Suit y)
        {
            return x.CompareTo(y) >= 0;
        }

        public static bool operator <=(Suit x, Suit y)
        {
            return x.CompareTo(y) <= 0;
        }
    }
}