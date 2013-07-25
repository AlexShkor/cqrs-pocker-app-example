using AKQ.Domain.Documents;
using AKQ.Domain.Services;
using AKQ.Domain.ViewModel;

namespace AKQ.Domain.GameEvents
{
    public class GameCreated : GameEvent
    {
        public string HostId { get; set; }

        public DealTypeEnum GameType { get; set; }

        public string DealId { get; set; }

        public GameViewModel GameModel { get; set; }
    }
}