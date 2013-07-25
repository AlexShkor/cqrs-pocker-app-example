using AKQ.Domain.GameEvents;
using AKQ.Domain.Messaging;
using AKQ.Domain.UserEvents;
using Segmentio;
using Segmentio.Model;

namespace AKQ.Domain.EventHandlers
{
    [ProcessingOrder(QueueNamesEnum.Statistics, Order = 0)]
    public sealed class StatiscticsHandler: BridgeEventHandler
    {
        public StatiscticsHandler()
        {
            AddHandler((GameCreated e) =>
            {
                Analytics.Client.Track(e.HostId, "Game Created", new Properties()
                {
                    {"Game Id", e.GameId},
                    {"Deal Id", e.DealId},
                    {"Game Type", e.GameType},
                });
                //Analytics.Client.Identify(e.GameId, new Traits()
                //{
                //    {"Contract", e.GameModel.Contract.ToString()},
                //    {"Host Id", e.HostId},
                //    {"Deal Id", e.DealId},
                //    {"Game Type", e.GameType},
                //});
            }); 
            AddHandler((GameFinished e) => Analytics.Client.Track(e.HostId, "Game Finished", new Properties()
            {
                {"Game Id", e.GameId},
                {"Deal Id", e.DealId},
                {"Game PBN", e.GamePBN},
                {"Result", e.Result},
            })); 
            AddHandler((GameStarted e) => Analytics.Client.Track(e.HostId, "Game Started", new Properties()
            {
                {"Game Id", e.GameId},
                {"Contract", e.Contract.ToString()},
            }));
        }
    }
}