using System;
using AKQ.Domain.Documents;

namespace AKQ.Web.Models
{
    public class ProfileViewModel
    {
        public ProfileViewModel(User doc, long totalGames, long totalWons)
        {
            Id = doc.Id;
            Email = doc.Email;
            Username = doc.Username;
            TotalGames = totalGames;
            TotalWons = totalWons;
            TotalLoses = totalGames - totalWons;
            Percent = String.Format("{0:P2}", ((double)totalWons) / totalGames);
        }

        public long TotalWons { get; set; }

        public long TotalGames { get; set; }

        public long TotalLoses { get; set; }

        public string Percent { get; set; }

        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}