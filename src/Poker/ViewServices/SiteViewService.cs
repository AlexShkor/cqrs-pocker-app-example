using Poker.Databases;
using Poker.Platform.Mongo;
using Poker.Platform.ViewServices;
using Poker.Views;

namespace Poker.ViewServices
{
    public class SitesViewService : ViewService<SiteView>
    {
        public SitesViewService(MongoViewDatabase database)
            : base(database)
        {
        }

        protected override ReadonlyMongoCollection<SiteView> Items
        {
            get { return Database.Sites; }
        }

        public SiteView GetSite()
        {
            return GetById(SiteSettings.SiteId);
        }
    }
}