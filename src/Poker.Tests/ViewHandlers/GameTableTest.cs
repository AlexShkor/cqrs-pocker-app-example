using NUnit.Framework;

namespace Poker.Tests.ViewHandlers
{
    public class GameTableTest: BaseViewHandlerTest
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
    }
}