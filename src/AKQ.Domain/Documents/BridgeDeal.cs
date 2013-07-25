using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace AKQ.Domain.Documents
{
    public class BridgeDeal
    {
        public BridgeDeal()
        {
        }

        public BridgeDeal(IEnumerable<string> dealfinderlines)
        {
            var dict = dealfinderlines
               .Select(line => line.Split(new[] { " " }, StringSplitOptions.None))
               .ToDictionary(parts => parts[0], parts => parts[1]);

            BoardNumber = dict["B"];
            PBNHand = dict["H"];
            Vulnerable = dict["A"];
            BestContract = new ContractDocument(dict["C"]);
            
            DoubleDummyMakes = dict["M"];

            if (BestContract.Position == "N" || BestContract.Position == "S")
            {  
                DealType = DealTypeEnum.Attack;
            }
            if (BestContract.Position == "E" || BestContract.Position == "W")
            { 
                DealType = DealTypeEnum.Defence;
            }
            var arr = dict["R"].Split('+', '-');
            BestResult = new ResultDocument
            {
                Tricks = int.Parse(arr[0]),
                Points = int.Parse(arr[1])
            };
        }

        [BsonId]
        public string Id { get; set; }
        public DealTypeEnum DealType { get; set; }
        public string PBNHand { get; set; }
        public string BoardNumber { get; set; }
        public string Vulnerable { get; set; }
        public ContractDocument BestContract { get; set; }
        public ResultDocument BestResult { get; set; }
        public string DoubleDummyMakes { get; set; }
    }

    public class ResultDocument
    {
        public int Tricks { get; set; }
        public int Points { get; set; }
    }

    public enum DealTypeEnum
    {
        Attack,
        Defence
    }
}