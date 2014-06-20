using NUnit.Framework;
using Poker.Views;

namespace Poker.Tests.ViewHandlers
{
    public class GameTableTest : BaseViewHandlerTest
    {
        [Test]
        public void TableState()
        {
            Table.CreateTable("table1", "Table 1", 1000, 5);
            SendAllEvents();

            var table = View.Tables.GetById("table1");
            Assert.NotNull(table);
            Assert.AreEqual("Table 1", table.Name);
        }

        [Test]
        public void GameWithTwoPlayers()
        {
            View.Users.Save(new UserView() { Id = "me1", Cash = 1000 });
            View.Users.Save(new UserView() { Id = "me2", Cash = 1000 });

            Table.CreateTable("table1", "Table 1", 1000, 5);

            Table.JoinTable("me1", 1000);
            Table.JoinTable("me2", 1000);

            SendAllEvents();
            var table = View.Tables.GetById("table1");
            Assert.AreEqual(10, table.Players[0].Bid);
            Assert.AreEqual(5, table.Players[1].Bid);

            /* Pre-Flop */
            Table.Call("me2");
            Table.Call("me1");

            /* Flop */
            Table.Call("me2");
            Table.Call("me1");

            /* Turn */
            Table.Call("me2");
            Table.Call("me1");

            /* River */
            Table.Call("me2");
            Table.Call("me1");

            /* Game was finished, New game was created, Blinds were changed */

            SendAllEvents();
            table = View.Tables.GetById("table1");
            Assert.AreEqual(5, table.Players[0].Bid);
            Assert.AreEqual(10, table.Players[1].Bid);
        }



        [Test]
        public void GameWithThreePlayers()
        {
            View.Users.Save(new UserView() { Id = "me1", Cash = 1000 });
            View.Users.Save(new UserView() { Id = "me2", Cash = 1000 });
            View.Users.Save(new UserView() { Id = "me3", Cash = 1000 });

            Table.CreateTable("table1", "Table 1", 1000, 5);

            Table.JoinTable("me1", 1000);
            Table.JoinTable("me2", 1000);

            SendAllEvents();
            var table = View.Tables.GetById("table1");
            Assert.AreEqual(2, table.Players.Count);
            Assert.AreEqual(10, table.Players[0].Bid);
            Assert.AreEqual(5, table.Players[1].Bid);

            Table.JoinTable("me3", 1000);

            var gameId = Table.State.GameId;
            while (Table.State.GameId == gameId)
            {
                Table.Call("me2");
                Table.Call("me1");
            }

            /* Game was finished, New game was created, Blinds were changed */

            SendAllEvents();
            table = View.Tables.GetById("table1");
            Assert.AreEqual(3, table.Players.Count);
            Assert.AreEqual(10, table.Players[0].Bid);
            Assert.AreEqual(0, table.Players[1].Bid);
            Assert.AreEqual(5, table.Players[2].Bid);

            /* Pre-Flop */
            Table.Call("me2");
            Table.Call("me3");
            Table.Call("me1");

            /* Flop */
            Table.Call("me3");
            Table.Call("me1");
            Table.Call("me2");

            /* Turn */
            Table.Call("me3");
            Table.Call("me1");
            Table.Call("me2");

            /* River */
            Table.Call("me3");
            Table.Call("me1");
            Table.Call("me2");


            /* Game was finished, New game was created, Blinds were changed */

            SendAllEvents();
            table = View.Tables.GetById("table1");
            Assert.AreEqual(5, table.Players[0].Bid);
            Assert.AreEqual(10, table.Players[1].Bid);
            Assert.AreEqual(0, table.Players[2].Bid);
        }
    }
}