using System;
using System.Collections.Generic;
 

namespace AKQ.Domain.ViewModel
{
    public class SuitEstimateViewModel
    {
        private readonly Dictionary<PlayerPosition, EstimateItem> _players;

        public SuitEstimateViewModel(Suit suit, PlayerPosition editable, PlayerPosition estimated)
        {
            Suit = suit.ToShortName();
            Symbol = suit.ToSymbol();
            Color = suit.GetColor();
            _players = new Dictionary<PlayerPosition, EstimateItem>();
            Players = new List<EstimateItem>();
            Action<PlayerPosition> initPlayer = pos =>
            {
                var item = new EstimateItem()
                {
                    Position = pos.ToShortName(),
                    Editable =  pos == editable,
                    Estimated =  pos == estimated,
                };
                _players[pos] = item;
                Players.Add(item);
            };
            initPlayer(PlayerPosition.West);
            initPlayer(PlayerPosition.North);
            initPlayer(PlayerPosition.East);
            initPlayer(PlayerPosition.South);
        }

        public EstimateItem this[PlayerPosition pos]
        {
            get { return _players[pos]; }
        }

        public string Suit { get; set; }
        public string Symbol { get; set; }
        public string Color { get; set; }
        public List<EstimateItem> Players { get; set; }
    }
}