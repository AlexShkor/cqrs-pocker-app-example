using System.Dynamic;
using Microsoft.AspNet.SignalR;

namespace AKQ.Domain.Hubs
{
    public class UsersHub: Hub
    {
        private static IHubContext CuurentContext
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<UsersHub>();
            }
        }

        private static string GetUserGroup( string userId)
        {
            return "User_" + userId;
        }

        public void Connect(string userId)
        {
            Groups.Add(Context.ConnectionId, GetUserGroup(GetUserGroup(userId)));
        }

        public static void RedirectToTournament(string tournamentId, string userId)
        {
            CuurentContext.Clients.Group(GetUserGroup(userId)).redirectToTournament(new
            {
                Id = tournamentId
            });
        }
    }
}