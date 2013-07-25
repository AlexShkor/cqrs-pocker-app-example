using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using AKQ.Domain.Documents;
using AKQ.Domain.Infrastructure;
using AKQ.Domain.Services;
using AKQ.Web.Controllers;
using MongoDB.Bson;

namespace AKQ.Web.Authentication
{
    public class AuthenticationService
    {
        private readonly UsersService _users;
        private readonly CryptographicHelper _crypto;

        private readonly UserLogsService _usersLogs;

        public AuthenticationService(UsersService users, CryptographicHelper crypto, UserLogsService usersLogs)
        {
            _users = users;
            _crypto = crypto;
            _usersLogs = usersLogs;
            _crypto = crypto;
        }

        public User ValidateUser(string email, string password)
        {
            var user = _users.GetUserByCredentionals(email, _crypto.GetPasswordHash(password));

            return user;
        }

        public void LoginUser(User user, bool rememberMe = true)
        {
            var data = String.Format("{0}|{1}", user.Role, user.Username);

            var authTicket = new FormsAuthenticationTicket(
                10,
                user.Id,
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

            HttpContext.Current.Session[BaseController.SessionKeys.UserName] = user.Username;
            Task.Factory.StartNew(() => _usersLogs.Save(new UserLogDocument
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = user.Id,
                Action = UserLogActionEnum.LoggedIn,
                Date = DateTime.Now,
                UserName = user.Username
            }));
        }
    }
}