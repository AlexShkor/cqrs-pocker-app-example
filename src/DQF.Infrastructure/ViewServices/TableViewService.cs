using PAQK.Databases;
using PAQK.Documents;
using PAQK.Platform.Mongo;
using PAQK.Platform.ViewServices;
using PAQK.Views;

namespace PAQK.ViewServices
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