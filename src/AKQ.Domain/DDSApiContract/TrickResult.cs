using System.Collections.Generic;

namespace DdsContract
{
    public class TrickResult
    {
        public List<CardResult> West { get; set; }
        public List<CardResult> East { get; set; }
        public List<CardResult> North { get; set; }
        public List<CardResult> South { get; set; }

        public int Number { get; set; }

        public TrickResult()
        {
            West = new List<CardResult>();
            East = new List<CardResult>();
            South = new List<CardResult>();
            North = new List<CardResult>();
        }

        public List<CardResult> this[string player]
        {
            get
            {
                switch (player.ToUpper())
                {
                    case "W":
                        return West;
                    case "N":
                        return North;
                    case "S":
                        return South;
                    case "E":
                        return East;
                }
                return null;
            }
        }
    }
}