namespace AKQ.Web.Models
{
    public class WaitTournamentViewModel
    {
        public string TournamentId { get; set; }
        public int SecondsToStart { get; set; }
        public bool Registred { get; set; }
        public int Players { get; set; }
    }
}