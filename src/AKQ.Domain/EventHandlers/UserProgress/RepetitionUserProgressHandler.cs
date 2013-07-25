using AKQ.Domain.Documents.Progress;
using AKQ.Domain.Messaging;
using AKQ.Domain.Services;
using AKQ.Domain.UserEvents;

namespace AKQ.Domain.EventHandlers
{
    [ProcessingOrder(IsAsync = true)]
    public sealed class RepetitionUserProgressHandler : BridgeEventHandler
    {
        private readonly UserProgressService _progress;

        public RepetitionUserProgressHandler(UserProgressService progress)
        {
            _progress = progress;
            AddHandler((GameFinished e) =>
            {
                var doc = _progress.GetById(e.HostId);
                var deal = doc.RepetitionProgress.GetCurrentDeal();
                if (deal != null && deal.DealId == e.DealId)
                {
                    deal.DealGameStats.Add(new DealGameStat(e.GameId, e.Finished, e.Result));
                    _progress.Save(doc);
                }
            });
        }
    }
}