using PAQK.Databases;
using PAQK.Platform.Mongo;
using PAQK.Platform.ViewServices;
using PAQK.Views;

namespace PAQK.ViewServices
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