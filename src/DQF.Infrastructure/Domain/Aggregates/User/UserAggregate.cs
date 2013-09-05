using System;
using PAQK.Domain.Aggregates.User.Commands;
using PAQK.Domain.Aggregates.User.Events;
using PAQK.Platform.Domain;
using PAQK.Platform.Extensions;

namespace PAQK.Domain.Aggregates.User
{
    public class UserAggregate : Aggregate<UserState>
    {
        public void Create(
            string id, 
            string userName, 
            string passwordHash, 
            string passwordSalt, 
            string email,
            string facebookId)
        {
            if (State.Id.HasValue())
            {
                if (State.UserWasDeleted)
                {
                    Apply(new UserReCreated
                    {
                        Id = id,
                        UserName = userName,
                        Email = email,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        CreationDate = DateTime.Now
                    });
                }
                else
                {
                    throw new InvalidOperationException("User with same ID already exist.");
                }
            }
            else
            {
                Apply(new UserCreated
                {
                    Id = id,
                    UserName = userName,
                    Email = email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreationDate = DateTime.Now,
                    FacebookId = facebookId,
                    Cash = 5000
                });
            }
        }

        public void ChangePassword(string passwordHash, string passwordSalt, bool isChangedByAdmin)
        {

            Apply(new PasswordChanged
            {
                Id = State.Id,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                WasChangedByAdmin = isChangedByAdmin,
            });
        }

        public void Delete(DeleteUser c)
        {
            Apply(new UserDeleted
            {
                Id = c.Id,
                DeletedByUserId = c.DeletedByUserId,
            });
        }

        public void UpdateDetails(UpdateUserDetails c)
        {
            Apply(new UserDetailsUpdated
            {
                Id = c.Id,
                UserName = c.UserName
            });
        }
    }
}