using AKQ.Domain.Services;

namespace AKQ.Domain.ViewModel.Room
{
    public class RoomPlayerViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }

        public RoomPlayerViewModel(ConnectionView view)
        {
            UserId = view.ConntectionId;
            Name = view.Name;
            Time = view.ConnectedDateTime.ToShortTimeString();
        }
    }
}