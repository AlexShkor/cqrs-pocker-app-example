 

using System;

namespace AKQ.Domain
{
    [Serializable]
    public class Player
    {
        public Player(PlayerPosition position, string name = null)
        {
            _playerPosition = position;
            Name = name ?? position.ToString();
        }

        private readonly PlayerPosition _playerPosition;

        public PlayerPosition PlayerPosition { get { return _playerPosition; } }

        public string Name { get; set; }

        public string UserId { get; private set; }

        public bool IsDummy { get; set; }

        public PlayersManager PlayersManager { get; set; }

        public string ControledByUserId { get; private set; }

        public bool IsAI
        {
            get { return string.IsNullOrEmpty(ControledByUserId); }
        }

        public override string ToString()
        {
            return _playerPosition.ToShortName();
        }

        public void TakePlace(string userId, string username)
        {
            UserId = userId;
            ControledByUserId = userId;
            Name = username;
        }

        public void TakeControl(string userId)
        {
            ControledByUserId = userId;
        }

        public bool HasControl(string currentUserId)
        {
            return ControledByUserId == currentUserId;
        }
    }
}