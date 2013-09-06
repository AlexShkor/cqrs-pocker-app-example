using System.Security.Principal;

namespace PAQK.Authentication
{
    public class AkqPrincipal : IPrincipal
    {
        public AkqPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        public bool IsInRole(string role)
        {
            return true;
        }

        public IIdentity Identity { get; private set; }
    }
}