using System;
using System.Collections.Generic;
using AKQ.Domain.Documents;
using AKQ.Domain.Utils;

namespace AKQ.Web.Models
{
    public class TournamentsListViewModel
    {
        public List<TournamentItemViewModel> Items { get; set; }
    }

    public class TournamentItemViewModel
    {
        public TournamentItemViewModel(TournamentDocument doc)
        {
            Id = doc.Id;
            DurationInMinutes = doc.MinutesToPlay;
            HandsToPlay = doc.HandsToPlay;
            Started = doc.StartTime.ToRelativeDate();
            TimeLeft = (doc.ExpectedFinishAt.Value - DateTime.Now).ToString("mm") + " min";
        }

        public string Id { get; set; }
        public int DurationInMinutes { get; set; }
        public int HandsToPlay { get; set; }
        public string Started { get; set; }
        public string TimeLeft { get; set; }
    }
}