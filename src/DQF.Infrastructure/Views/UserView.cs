using System;
using MongoDB.Bson.Serialization.Attributes;
using Uniform;

namespace PAQK.Views
{
    public class UserView
    {
        [DocumentId, BsonId]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreationDate { get; set; }

        public UserView()
        {
        }
    }
}