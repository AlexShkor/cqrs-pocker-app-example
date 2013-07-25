using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace AKQ.Domain.Documents
{
    public class BridgeGameDocument
    {
        [BsonId]
        public string Id { get; set; }

        public List<TrickDocument> Tricks { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Started { get; set; }

        public string HostUserId { get; set; }

        public string DealId { get; set; }

        public string OridinalHandsPBN { get; set; }

        public List<HandDocument> Hands { get; set; }

        public ContractDocument Contract { get; set; }

        public DealTypeEnum GameType { get; set; }

        public DateTime? Finished { get; set; }

        public List<PlayerDocument> JoinedPlayers { get; set; }

        public string PBN { get; set; }
        public int Result { get; set; }

        public string BoardNumber { get; set; }

        public string RoboBridgePBN { get; set; }

        public string HostUserName { get; set; }

        public string Notes { get; set; }

        public BridgeGameDocument()
        {
            Tricks = new List<TrickDocument>();
            JoinedPlayers = new List<PlayerDocument>();
            Hands = new List<HandDocument>();
        }
    }
}