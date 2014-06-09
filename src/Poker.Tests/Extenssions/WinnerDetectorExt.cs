using System.Collections.Generic;
using Poker.Domain.Aggregates.Game;
using Poker.Domain.ApplicationServices;

namespace Poker.Tests
{
    public static class WinnerDetectorExt
    {
        public static IEnumerable<WinnerResult> GetWinners(this WinnerDetector source)
        {
            return source.GetWinners(100);
        }
    }
}