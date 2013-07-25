namespace AKQ.Domain.ViewModel
{
    public class PlayerViewModel
    {
        public PlayerViewModel(Player player)
        {
            Position = player.ToString();
            Name = player.Name;
            UserId = player.UserId;
        }

        public string UserId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
    }
}