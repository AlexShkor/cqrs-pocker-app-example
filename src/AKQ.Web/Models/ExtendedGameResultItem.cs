using System.Collections.Generic;
using System.Linq;
using AKQ.Domain;
using AKQ.Domain.Documents;
using AKQ.Domain.Utils;

namespace AKQ.Web.Models
{
    public class ExtendedGameResultItem:GameResultItem
    {
        public string Tags { get; set; }
        public string BoardNumber { get; set; }
        public string DealId { get; set; }
        public int ContractValue { get; set; }

        public string ContractSuitHtml { get; set; }

        public string ContractSuitColor { get; set; }

        public string ContractSuitFontSize { get { return ContractSuitHtml == "NT" ? "inherit" : "1.2em"; } }

        public ExtendedGameResultItem(BridgeGameDocument x, User user)
        {
            BoardNumber = x.BoardNumber ?? "";
            DealId = x.DealId;
            Tags = string.Join(", ", (user.GetTags(x.DealId) ?? new List<Tag>()).Select(tag => tag.Title));
            ContractValue = x.Contract.Value;
            ContractSuitHtml = Suit.FromShortName(x.Contract.Suit).Html;
            ContractSuitColor = Suit.FromShortName(x.Contract.Suit).HtmlColor;
            GameId = x.Id;
            GameDate = x.Finished.Value.ToRelativeDate();
            Result = ViewDataFormatter.GameResult(x.Result);
            CssClass = "alert-" + (x.Result >= 0 ? "success" : "error");
        }
    }
}