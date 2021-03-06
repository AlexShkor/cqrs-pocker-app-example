﻿using System.Collections.Generic;
using Poker.Platform.Domain.Transitions;

namespace Poker.Platform.Upgrade
{
    public interface IUpgrader
    {
        int Number { get; }

        bool IsEnabled { get; }

        /// <summary>
        /// This method will be called for each transition in EventStore.
        /// Order will be according to Timestamp property of transitions, ascending.
        /// </summary>
        IEnumerable<Transition> Upgrade(Transition transition);
    }
}