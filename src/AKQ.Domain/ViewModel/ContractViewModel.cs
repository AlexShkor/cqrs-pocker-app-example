using System;
 

namespace AKQ.Domain.ViewModel
{
    public class ContractViewModel
    {
        public string Value { get; set; }
        public string Symbol { get; set; }
        public string Color { get; set; }
        public string Suit { get; set; }
        public string Declarer { get; set; }

        public string NsTarget { get; set; }
        public string EwTarget { get; set; }

        public ContractViewModel()
        {
            
        }

        public ContractViewModel(Contract contract)
        {
            Value = contract.Value.ToString();
            Symbol = contract.Suit.ToSymbol();
            Color = contract.Suit.GetColor();
            Suit = contract.Suit.ToShortName();
            Declarer = contract.Declarer.ToString();
            var target = contract.GetTarget();
            if (contract.Declarer.IsNorthSouthTeam)
            {
                NsTarget = target.ToString();
            }
            if (contract.Declarer.IsEastWestTeam)
            {
                EwTarget = target.ToString();
            }
        }
    }
}