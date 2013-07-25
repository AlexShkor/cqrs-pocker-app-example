using System.Collections.Generic;
using System.Linq;
using PBN;

namespace AKQ.Domain.Documents
{
    public class HandDocument
    {
        public string Position { get; set; }

        public List<CardDocument> Cards { get; set; }

        public HandDocument() { }

        public HandDocument(Hand hand)
        {
            Cards = hand.GetAll().Select(x => new CardDocument(x)).ToList();
            Position = hand.Position.ShortName;
        }

        public HandDocument(HandParseResult hand)
        {
            Cards = hand.Cards.Select(x => new CardDocument
            {
                Suit = x.Substring(0,1),
                Value = x.Substring(1).Replace("T","10")
            }).ToList();
            Position = hand.Position;
        }
    }
}