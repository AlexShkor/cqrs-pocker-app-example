using System;
using System.Threading;
using AKQ.Domain.Documents;
 
using AKQ.Domain.RoboAI;
using System.Linq;
using DdsContract;

namespace AKQ.Domain.Services
{
    public interface IRoboBridgeAI
    {
        ArrayOfString PossibleBids(string gamePBN);
        string GetBid(string gamePBN, PlayerPosition seat);
        string GetCard(string gamePBN, PlayerPosition seat);
    }

    public class RoboBridgeAI : IRoboBridgeAI
    {
        public ArrayOfString PossibleBids(string gamePBN)
        {
            var aiClient = new RoboAIClient();
            return aiClient.PossibleBids(gamePBN, "SAYC");
        }

        public string GetBid(string gamePBN, PlayerPosition seat)
        {
            var aiClient = new RoboAIClient();
            return aiClient.GetBid(gamePBN, "SAYC", seat.ToShortName());
        }

        public string GetCard(string gamePBN, PlayerPosition seat)
        {
            var aiClient = new RoboAIClient();
            return aiClient.GetCard(gamePBN, "SAYC", seat.ToShortName(), 0, 10);
        }
    }


    public class DDSRemoteAI : IRoboBridgeAI
    {

        public ArrayOfString PossibleBids(string gamePBN)
        {
            throw new NotImplementedException();
        }

        public string GetBid(string gamePBN, PlayerPosition seat)
        {
            throw new NotImplementedException();
        }

        public string GetCard(string gamePBN, PlayerPosition seat)
        {
            var aiClient = new DdsApiClient();
            Thread.Sleep(200);
            var card = aiClient.GetCard(gamePBN);
            return card.Suit + card.Rank;
        }
    }
}