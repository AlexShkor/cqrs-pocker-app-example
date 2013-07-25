namespace AKQ.Web.Models
{
    public class AddNotesViewModel 
    {
        public string GameId { get; set; }
        public string Notes { get; set; }

        public AddNotesViewModel()
        {
            
        }

        public AddNotesViewModel(string gameId, string notes)
        {
            GameId = gameId;
            Notes = notes;
        }
    }
}