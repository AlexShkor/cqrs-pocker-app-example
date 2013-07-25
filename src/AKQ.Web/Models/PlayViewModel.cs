namespace AKQ.Web.Models
{
    public class PlayViewModel
    {
        public string Card { get; set; }
        public string Position { get; set; }
        public bool IsWon { get; set; }
        public bool IsWrong { get; set; }

        public string CssClass{get { return (IsWon ? "won-trick" : "") + (IsWrong ? "red-round-border" : ""); }}

        public string Tip { get; set; }
    }
}