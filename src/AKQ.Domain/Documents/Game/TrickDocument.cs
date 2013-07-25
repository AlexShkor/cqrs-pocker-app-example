using System.Collections.Generic;
using System.Linq;

namespace AKQ.Domain.Documents
{
    public class TrickDocument
    {
        public List<PlayerCardDocument> Cards { get; set; }

        public int TrickNumber { get; set; }

        public string Winner { get; set; }

        public TrickDocument()
        {
            Cards = new List<PlayerCardDocument>();
        }

        public TrickDocument(Trick trick, int trickNumber)
        {
            TrickNumber = trickNumber;
            Cards = trick.PlayedCards.Select(x => new PlayerCardDocument(x.Value, x.Key)).ToList();
            Winner = trick.Winner.ToShortName();
        }
    }
}