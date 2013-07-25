using System;
using System.Collections.Generic;
using System.Linq;
using MS.Internal.Xml.XPath;
using MongoDB.Bson.Serialization.Attributes;

namespace AKQ.Domain.Documents
{
    public class User
    {
        private DateTime? _registred;

        [BsonId]
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public string Role { get; set; }
        public string FacebookId { get; set; }

        public DateTime Registred
        {
            get { return _registred ?? DateTime.Now; }
            set { _registred = value; }
        }

        public Dictionary<string,DealTags> SavedTags { get; set; }

        public string ConnectionToken { get; set; }

        public User(string email, string username, string hashedPassword): this()
        {
            Email = email;
            HashedPassword = hashedPassword;
            Username = username;
        }

        public User()
        {
            SavedTags = new Dictionary<string, DealTags>();
            Registred = DateTime.Now;
        }

        public void UpdateTags(DealTags item)
        {
            SavedTags[item.DealId] = item;
        }

        public List<Tag> GetTags(string dealId)
        {
            if (SavedTags.ContainsKey(dealId))
            {
                return SavedTags[dealId].Tags;
            }
            return null;
        }
    }

    public class DealTags
    {
        public string DealId { get; set; }

        public List<Tag> Tags { get; set; }

        public DealTags()
        {
            Tags = new List<Tag>();
        }
    }
}