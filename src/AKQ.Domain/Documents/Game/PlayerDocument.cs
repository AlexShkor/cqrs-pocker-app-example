namespace AKQ.Domain.Documents
{
    public class PlayerDocument
    {
        public string Position { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }

        public PlayerDocument()
        {

        }

        public PlayerDocument(PlayerPosition position, string userId, string name)
        {
            Position = position.ToShortName();
            UserId = userId;
            Name = name;
        }
    }
}