﻿using System;
using Poker.Platform.Domain.Messages;

namespace Poker.Domain.Aggregates.User.Events
{
    public class UserCreated : Event
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreationDate { get; set; }
        public long Cash { get; set; }
        public string FacebookId { get; set; }
    }
}