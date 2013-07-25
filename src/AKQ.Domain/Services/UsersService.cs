using System.Collections.Generic;
using System.Text.RegularExpressions;
using AKQ.Domain.Documents;
using AKQ.Domain.Infrastructure.Mongodb;
using AKQ.Domain.Infrastructure.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AKQ.Domain.Services
{
    public class UsersService : ViewServiceFiltered<User, UserFilter>
    {
        public UsersService(MongoDocumentsDatabase database)
            : base(database)
        {

        }

        protected override MongoCollection<User> Items
        {
            get { return Database.Users; }
        }

        public User GetUserByCredentionals(string email, string passwordHash)
        {
            return Items.FindOne(Query.And(Query<User>.EQ(x => x.Email, email.ToLower()), Query<User>.EQ(x => x.HashedPassword, passwordHash)));
        }

        public User GetByEmail(string email)
        {
            return Items.FindOne(Query<User>.EQ(x => x.Email, email.ToLower()));
        }

        public User GetUserName(string userName)
        {
            return Items.FindOne(Query<User>.EQ(x => x.Username, userName));
        }

        protected override IEnumerable<IMongoQuery> BuildFilterQuery(UserFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.SearchKey))
            {
                var bsonRegex = BsonRegularExpression.Create(new Regex(filter.SearchKey, RegexOptions.IgnoreCase));
                yield break;
            }
        }

        public void SetConnectionToken(string id, string token)
        {
            var user = GetById(id);
            user.ConnectionToken = token;
        }

        public User GetByFacebookId(string facebookId)
        {
            return Items.FindOneAs<User>(Query<User>.EQ(x => x.FacebookId, facebookId));
        }

        public IEnumerable<User> GetAllSorted()
        {
            return Items.FindAll().SetSortOrder(SortBy<User>.Ascending(x=> x.Id));
        }
    }

    public class UserFilter : BaseFilter
    {
        public string SearchKey { get; set; }
    }
}