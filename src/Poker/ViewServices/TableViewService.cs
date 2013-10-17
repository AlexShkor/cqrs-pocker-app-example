using Poker.Databases;
using Poker.Platform.Mongo;
using Poker.Platform.ViewServices;
using Poker.Views;

namespace Poker.ViewServices
{
    public class TableViewService: ViewService<TableView>
    {
        public TableViewService(MongoViewDatabase database) : base(database)
        {
        }

        protected override ReadonlyMongoCollection<TableView> Items
        {
            get { return Database.Tables; }
        }
    }
}