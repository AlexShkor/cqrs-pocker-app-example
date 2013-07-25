using System.Collections.Generic;
using System.Linq;
using AKQ.Domain;
using AKQ.Domain.Documents;
using AKQ.Domain.ViewModel;
using AKQ.Web.Controllers;
using PBN;

namespace AKQ.Web.Models
{
    public class ReplayViewModel
    {
        public string GameId { get; set; }
        public ContractViewModel Contract { get; set; }
        public Dictionary<string, HandViewModel> Hands { get; set; }
        public List<ExplainedTrickViewModel> Tricks { get; set; }

        public TacticsViewModel Tactics { get; set; }

        public string PlayerName { get; set; }

        public string Result { get; set; }

        public string DealId { get; set; }

        public string DownloadUrl { get; set; }

        public ReplayViewModel()
        {
            Tactics = new TacticsViewModel();
        }

        public ReplayViewModel(BridgeGameDocument doc, User user)
            : this()
        {
            var positionsNames = new Dictionary<string, string>
            {
                {"S", doc.HostUserName},
                {"W", ""},
                {"E", ""},
                {"N", ""}
            };
            var suitsOrder = new Dictionary<string, int>
            {
                {"S", 1},
                {"H", 2},
                {"C", 3},
                {"D", 4},
            };

            var parse = PbnParser.ParseGame(doc.PBN);
            var hands = PbnParser.ParseHands(parse.Deal).ToDictionary(key => key.Position, x => new HandViewModel
            {
                HasControl = false,
                IsVisible = true,
                PlayerName = positionsNames[x.Position],
                Cards = x.Cards.GroupBy(c => c.Substring(0, 1)).OrderBy(c => suitsOrder[c.Key]).Select(g => g.Select(v => new CardViewModel(Card.FromString(v), false)).ToList()).ToList()
            });
            var order = new List<string> { "W", "N", "E", "S" };

            Hands = hands;
            GameId = doc.Id;
            DealId = doc.DealId;
            DownloadUrl = "/home/download/" + doc.Id;
            Contract = new ContractViewModel(doc.Contract.ToDomainObject());
            PlayerName = doc.HostUserName;
            Result = ViewDataFormatter.GameResult(doc.Result);
            Tricks = doc.Tricks.Select(x => new ExplainedTrickViewModel(x.TrickNumber)
            {
                Players = x.Cards.OrderBy(o => order.IndexOf(o.Player)).Select(card => new PlayViewModel
                {
                    Card = card.Suit + card.Value,
                    IsWon = card.Player == x.Winner,
                    Position = card.Player
                }).ToList()
            }).ToList();
            Tactics = new TacticsViewModel(user.GetTags(doc.DealId) ?? new List<Tag>());
        }
    }

    public class TacticsViewModelPost
    {
        public string DealId { get; set; }
        public List<TagViewModel> Tags { get; set; }

        public TacticsViewModelPost()
        {
            Tags = new List<TagViewModel>();
        }
    }
}