using Poker.Domain.Aggregates.Game;
using Poker.Domain.Aggregates.Game.Data;
using Poker.Domain.Aggregates.Game.Events;

namespace Poker.Tests.Extenssions
{
    public static class GameTableExt
    {
        public static void Join3Players(this GameTableAggregate a)
        {
            a.JoinTable("me1", 100 + 4);
            a.JoinTable("me2", 100 + 2);
            a.JoinTable("me3", 100);
            a.Apply(new GameFinished
            {
                Id = "123",
                Bank = 0,
                Winner = new PlayerInfo
                {
                    UserId = "me1",
                    Position = 1
                }
            });
            a.CreateGame("game2");
        }
    }
}