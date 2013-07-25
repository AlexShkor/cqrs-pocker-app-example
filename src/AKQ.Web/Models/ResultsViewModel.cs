using System.Collections.Generic;
using System.Linq;
using AKQ.Domain.Documents;
using AKQ.Domain.Documents.Progress;
using AKQ.Domain.Utils;

namespace AKQ.Web.Models
{
    public class ResultsViewModel
    {
        public List<DealResultItem> Items { get; set; }  
    }

    public class DealResultItem
    {
        public string Tags { get; set; }
        public string BoardNumber { get; set; }
        public string DealId { get; set; }
        public int ContractValue { get; set; }

        public string ContractSuitHtml { get; set; }

        public string ContractSuitColor { get; set; }

        public string ContractSuitFontSize { get { return ContractSuitHtml == "NT" ? "inherit" : "1.2em"; } }

        public List<GameResultItem> Games { get; set; }

        public DealResultItem()
        {
            Games = new List<GameResultItem>();
        }

        public DealResultItem(DealStats dealStats, List<Tag> tags)
        {
            var contract = dealStats.Contract.ToDomainObject();
            DealId = dealStats.DealId;
            BoardNumber = dealStats.BoardNumber;
            ContractValue = contract.Value;
            ContractSuitColor = contract.Suit.HtmlColor;
            ContractSuitHtml = contract.Suit.Html;
            Games = dealStats.DealGameStats.Select(x => new GameResultItem(x)).ToList();
            Tags = string.Join(", ", (tags ?? new List<Tag>()).Select(x => x.Title));
        }
    }

    public class GameResultItem
    {
        public string Result { get; set; }
        public string GameDate { get; set; }
        public string GameId { get; set; }
        public string CssClass { get; set; }

        public GameResultItem(DealGameStat game)
        {
            GameId = game.GameId;
            GameDate = game.Finished.ToRelativeDate();
            Result = ViewDataFormatter.GameResult(game.Result);
            CssClass = "alert-" + (game.Result >= 0 ? "success" : "error");
        }

        public GameResultItem()
        {
            
        }
    }
}