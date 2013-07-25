using System;

namespace AKQ.Domain
{
    [Serializable]
    public struct Contract
    {
        private readonly int _value;
        private readonly Suit _suit;
        private readonly PlayerPosition _declarer;

        public Contract(int value, Suit suit, PlayerPosition declarer)
        {
            _value = value;
            _suit = suit;
            _declarer = declarer;
        }

        public PlayerPosition Declarer
        {
            get { return _declarer; }
        }

        public Suit Suit
        {
            get { return _suit; }
        }

        public int Value
        {
            get { return _value; }
        }

        public int GetTarget()
        {
            return _value + 6;
        }

        public override string ToString()
        {
            return Value + Suit.ShortName;
        }
    }
}