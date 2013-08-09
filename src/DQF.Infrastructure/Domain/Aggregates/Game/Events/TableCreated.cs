﻿using PAQK.Platform.Domain.Messages;

namespace PAQK.Domain.Aggregates.Game.Events
{
    public class TableCreated: Event
    {
        public string Name { get; set; }

        public long BuyIn { get; set; }

        public long SmallBlind { get; set; }

        public int MaxPlayers { get; set; }
    }
}