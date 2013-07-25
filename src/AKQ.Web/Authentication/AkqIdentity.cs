using System.Security.Principal;
using AKQ.Domain.Documents;

namespace AKQ.Web.Authentication
{
    public class AkqIdentity : IIdentity
    {
        public AkqIdentity(User user)
        {
            User = user;
        }

        public User User { get; set; }

        public string Name
        {
            get { return User.Username; }
        }

        public string AuthenticationType
        {
            get { return "Forms"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }
    }
}