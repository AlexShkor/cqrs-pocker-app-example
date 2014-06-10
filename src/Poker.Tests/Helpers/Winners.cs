using System.Collections.Generic;
using Poker.Domain.Aggregates.Game.Data;

namespace Poker.Tests
{
    public static class Winners
    {
        public static List<WinnerInfo> Me1(long prize)
        {
            return new List<WinnerInfo> {new WinnerInfo("me1", 1, prize, 1, 1)};
        }
    }
}