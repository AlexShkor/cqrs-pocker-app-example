using System.Security.Principal;
using PAQK.Views;

namespace AKQ.Web.Authentication
{
    public class AkqIdentity : IIdentity
    {
        private readonly string _email;
        private readonly string _username;

        public AkqIdentity(string email, string username)
        {
            _email = email;
            _username = username;
        }

        public string Name
        {
            get { return _username; }
        }

        public string Email
        {
            get { return _email; }
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