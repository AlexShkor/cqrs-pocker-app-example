using System.Security.Principal;

namespace AKQ.Web.Authentication
{
    public class AkqPrincipal : IPrincipal
    {
        public AkqPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        public bool IsInRole(string role)
        {
            return role == ((AkqIdentity)Identity).User.Role;
        }

        public IIdentity Identity { get; private set; }
    }
}