﻿using PAQK.Domain.Aggregates.User.Commands;
using PAQK.Helpers;
using PAQK.Platform.Dispatching.Interfaces;
using PAQK.Platform.Domain.Interfaces;

namespace PAQK.Domain.Aggregates.User
{
    public class UserApplicationService : IMessageHandler
    {
        private readonly IRepository<UserAggregate> _repository;
        private readonly CryptographicHelper _crypto;

        public UserApplicationService(IRepository<UserAggregate> repository,CryptographicHelper crypto)
        {
            _repository = repository;
            _crypto = crypto;
        }

        public void Handle(CreateUser c)
        {
            var salt = _crypto.GenerateSalt();
            var id = _crypto.GetMd5Hash(c.Email);
            _repository.Perform(id, user => user.Create(
                id,
                c.UserName,
                _crypto.GetPasswordHash(c.Password,salt),
                salt,
                c.Email,
                c.FacebookId));
        }

        public void Handle(ChangePassword c)
        {
            _repository.Perform(c.Id, user => user.ChangePassword(c));
        }

        public void Handle(DeleteUser c)
        {
            _repository.Perform(c.Id, user => user.Delete(c));
        }

        public void Handle(UpdateUserDetails c)
        {
            _repository.Perform(c.Id, user => user.UpdateDetails(c));
        }
    }
}