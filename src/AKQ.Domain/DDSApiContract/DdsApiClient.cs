using System.Configuration;
using DdsContract;
using ServiceStack.ServiceClient.Web;

namespace DdsContract
{
    public class DdsApiClient
    {
        private readonly JsonServiceClient _client;

        public DdsApiClient()
        {
            _client = new JsonServiceClient(ConfigurationManager.AppSettings["DdsApiUrl"]);
        }

        public SolveGameResponse SolveGame(string pbn)
        {
            return _client.Post(new SolveGame { PBN = pbn });
        }

        public GetAllCardsResponse GetAllCards(string pbn)
        {
            return _client.Post(new GetAllCards { PBN = pbn });
        }

        public GetCardResponse GetCard(string pbn)
        {
            return _client.Post(new GetCard{ PBN = pbn });
        }

        public PlayGameResponse PlayGame(string pbn)
        {
            return _client.Post(new PlayGame{ PBN = pbn });
        }
    }
}