using System.Collections.Generic;
using PAQK.Databases;
using PAQK.Platform.Mongo;

namespace PAQK.Platform.ViewServices
{
    public abstract class ViewService<T>
    {
        protected readonly MongoViewDatabase Database;

        protected ViewService(MongoViewDatabase database)
        {
            Database = database;
        }

        protected abstract ReadonlyMongoCollection<T> Items { get; }

        public virtual T GetById(string id)
        {
            return Items.FindOneById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Items.FindAll();
        }
    }
}
