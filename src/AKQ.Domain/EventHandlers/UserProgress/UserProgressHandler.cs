using AKQ.Domain.Documents.Progress;
using AKQ.Domain.Messaging;
using AKQ.Domain.Services;
using AKQ.Domain.UserEvents;

namespace AKQ.Domain.EventHandlers
{
    [ProcessingOrder(IsAsync = true)]
    public sealed class UserProgressHandler : BridgeEventHandler
    {
        private readonly UserProgressService _progress;

        public UserProgressHandler(UserProgressService progress)
        {
            _progress = progress;
            AddHandler((GameFinished e) =>
            {
                var doc = _progress.GetById(e.HostId);
                var gameSet = doc.PracticeProgress.GetGameSetFor(e.DealId);
                if (gameSet != null && !gameSet.GetDeal(e.DealId).HasWin())
                {
                    var deal = gameSet.GetDeal(e.DealId);
                    deal.DealGameStats.Add(new DealGameStat(e.GameId, e.Finished, e.Result));
                    _progress.Save(doc);
                }
            });
        }
    }
}