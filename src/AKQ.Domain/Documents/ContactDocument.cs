namespace AKQ.Domain.Documents
{
    public class ContractDocument
    {
        public int Value { get; set; }
        public string Suit { get; set; }
        public string Position { get; set; }

        public ContractDocument()
        {
            
        }

        public Contract ToDomainObject()
        {
            return new Contract(Value, Domain.Suit.FromShortName(Suit), PlayerPosition.FromShortName(Position));
        }

        public ContractDocument(Contract contract)
        {
            Value = contract.Value;
            Suit = contract.Suit.ToShortName();
            Position = contract.Declarer.ToShortName();
        }


        public ContractDocument(string parse)
        {
            var arr = parse.Split(':');
            Position = arr[1];
            Value = int.Parse(arr[0][0].ToString());
            Suit = arr[0][1].ToString().Replace("M", "7").Replace("N", "NT");
        }

        public string GetValueAndSuit()
        {
            return Value + Suit;
        }
    }
}