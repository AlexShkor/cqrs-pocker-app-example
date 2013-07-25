using AKQ.Domain.Services;
using AKQ.Domain.ViewModel;

namespace AKQ.Domain.UserEvents
{

    public class CardPlayed : GameEvent
    {
        public PlayerPosition Player { get; set; }
        public bool IsLast { get; set; }
        public Card Card { get; set; }
    }
}