using AKQ.Domain.Documents;

namespace AKQ.Web.Models
{
    public class OtherGameViewModel
    {

        public OtherGameViewModel(BridgeGameDocument doc)
        {
            Id = doc.Id;
            var contract = doc.Contract.ToDomainObject();
            Result = ViewDataFormatter.GameResult(doc.Result);
            ContractSuit = contract.Suit.Html;
            ContractSuitColor = contract.Suit.HtmlColor;
            ContractValue = doc.Contract.Value;
            PlayerName = doc.HostUserName;
            Date = doc.Started.Value.ToShortDateString();
        }

        public OtherGameViewModel(TournamentGameInfo doc)
        {
            Id = doc.GameId;
            var contract = doc.Contract.ToDomainObject();
            Result = doc.GetFormatedResult();
            ContractSuit = contract.Suit.Html;
            ContractSuitColor = contract.Suit.HtmlColor;
            ContractValue = doc.Contract.Value;
            PlayerName = doc.UserName;
            Date = doc.Started.ToShortDateString();
        }

        public string Result { get; set; }
        public string ContractSuitColor { get; set; }
        public string Id { get; set; }
        public string PlayerName { get; set; }
        public string ContractSuit { get; set; }
        public string Date { get; set; }
        public int ContractValue { get; set; }
    }
}