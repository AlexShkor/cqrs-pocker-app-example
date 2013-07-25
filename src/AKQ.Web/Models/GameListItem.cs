using AKQ.Domain.Documents;
using AKQ.Domain.Utils;

namespace AKQ.Web.Models
{
    public class GameListItem
    {
        public GameListItem(BridgeGameDocument doc)
        {
            Id = doc.Id;
            UserName = doc.HostUserName;
            Created = doc.Created.ToRelativeDate();
        }

        public string Created { get; set; }

        public string Id { get; set; }

        public string UserName { get; set; }
    }
}