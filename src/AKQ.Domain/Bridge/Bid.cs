using System;

namespace AKQ.Domain
{
    [Serializable]
    public struct Bid
    {
        private enum BidTypeEnum
        {
            Pass,
            Double,
            ReDouble,
            Set
        }

        private readonly BidTypeEnum _bidType;
        private readonly int _value;
        private readonly Suit _suit;

        public static readonly Bid Pass = new Bid(BidTypeEnum.Pass);
        public static readonly Bid Double = new Bid(BidTypeEnum.Double);
        public static readonly Bid ReDouble = new Bid(BidTypeEnum.ReDouble);

        private Bid(BidTypeEnum bidType):this()
        {
            _bidType = bidType;
        }

        public Bid(int value, Suit suit)
        {
            _value = value;
            _suit = suit;
            _bidType = BidTypeEnum.Set;
        }

        public int Value
        {
            get { return _value; }
        }

        public Suit Suit
        {
            get { return _suit; }
        }

        public bool HasValue
        {
        get { return _bidType == BidTypeEnum.Set; }
        }

        public static Bid FromString(string bid)
        {
            switch (bid.ToLower())
            {
                case "pass":
                    return Pass;
                case "double":
                    return Double;
                case "redouble":
                    return ReDouble;
                default:
                    return new Bid(int.Parse(bid.Substring(0, 1)), Suit.FromShortName(bid.Substring(1)));
            }
        }

        public override string ToString()
        {
            if (_bidType == BidTypeEnum.Set)
            {
                return _value + Suit.ToShortName();
            }
            return _bidType.ToString();
        }

        public static bool operator ==(Bid x, Bid y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Bid x, Bid y)
        {
            return !x.Equals(y);
        }

        public bool Equals(Bid other)
        {
            return _bidType.Equals(other._bidType) && _value == other._value && _suit.Equals(other._suit);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Bid && Equals((Bid)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = _bidType.GetHashCode();
                if (_bidType == BidTypeEnum.Set)
                {
                    hashCode = (hashCode*397) ^ _value;
                    hashCode = (hashCode*397) ^ _suit.GetHashCode();
                }
                return hashCode;
            }
        }
    }
}