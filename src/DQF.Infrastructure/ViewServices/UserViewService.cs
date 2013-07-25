﻿using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using PAQK.Databases;
using PAQK.Helpers;
using PAQK.Platform.Extensions;
using PAQK.Platform.Mongo;
using PAQK.Platform.ViewServices;
using PAQK.Views;

namespace PAQK.ViewServices
{
    public class UsersViewService : ViewServiceFiltered<UserView, UserFilter>
    {
        private readonly CryptographicHelper _crypto;

        public UsersViewService(MongoViewDatabase database, CryptographicHelper crypto)
            : base(database)
        {
            _crypto = crypto;
        }

        protected override ReadonlyMongoCollection<UserView> Items
        {
            get { return Database.Users; }
        }

        public UserView GetByUserName(string userName)
        {
            return Items.FindOne(Query<UserView>.EQ(x => x.UserName, userName));
        }

        public UserView GetByEmail(string email)
        {
            return GetById(_crypto.GetMd5Hash(email));
        }

        public override IEnumerable<IMongoQuery> BuildFilterQuery(UserFilter filter)
        {
            
                if (filter.UserId.HasValue())
                {
                    yield return Query<UserView>.EQ(x => x.Id, filter.UserId);
                }
        }

        
    }


    public class UserFilter : BaseFilter
    {
        public string UserId { get; set; }
    }
}