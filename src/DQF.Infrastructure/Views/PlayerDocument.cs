using System.Collections.Generic;
using AKQ.Domain;
using MongoDB.Bson.Serialization.Attributes;
using Uniform;

namespace PAQK.Views
{
 
    public class PlayerDocument
    {
        public int Position { get; set; }
        public long Cash { get; set; }
        public List<Card> Cards { get; set; }
        public long Bid { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
    }
}