using System.Collections.Generic;
using AKQ.Domain.ViewModel.Room;

namespace AKQ.Web.Models
{
    public class RoomViewModel
    {
        public List<RoomPlayerViewModel> Players { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Token { get; set; }
    }
}