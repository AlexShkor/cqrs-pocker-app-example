using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using AKQ.Domain.Infrastructure;
using AKQ.Domain.Services;
using AKQ.Web.Controllers;
using MongoDB.Bson;
using PAQK.Helpers;
using PAQK.Views;
using PAQK.ViewServices;

namespace AKQ.Web.Authentication
{
    public class AuthenticationService
    {
        private readonly UsersViewService _users;
        private readonly CryptographicHelper _crypto;

        public AuthenticationService(UsersViewService users, CryptographicHelper crypto)
        {
            _users = users;
            _crypto = crypto;
            _crypto = crypto;
        }

        public UserView ValidateUser(string email, string password)
        {
            var user = _users.GetByEmail(email);
            if (user.PasswordHash == _crypto.GetPasswordHash(password, user.PasswordSalt))
            {
                return user;
            }
            return null;
        }

        public void LoginUser(string email, string username, bool rememberMe = true)
        {
            var data = String.Format("{0}", username);

            var authTicket = new FormsAuthenticationTicket(
                10,
                _crypto.GetMd5Hash(email),
                DateTime.Now,
                DateTime.Now.AddDays(14),
                rememberMe,
                data);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            if (HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName] != null)
                HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
 
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            authCookie.Expires = DateTime.Now.AddDays(180);
            HttpContext.Current.Response.Cookies.Add(authCookie);

            HttpContext.Current.Session[BaseController.SessionKeys.UserName] = username;
        }
    }
}