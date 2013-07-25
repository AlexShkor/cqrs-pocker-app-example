using System;

namespace AKQ.Domain
{
    public class TournamentInfo
    {
        public string Id { get; set; }
        public int HandsToPlay { get; set; }
        public int MinutesToPlay{ get; set; }
        public DateTime? TournamentStarted { get; set; }
        public DateTime? GameStarted { get; set; }
        public bool IsLastGame { get; set; }
        public int TableNumber { get; set; }

        public TimeSpan GetTimeToPlay()
        {
            if (TournamentStarted.HasValue)
            {
                return TournamentStarted.Value.AddMinutes(MinutesToPlay) - DateTime.Now;
            }
            return TimeSpan.FromMinutes(MinutesToPlay);
        }
    }
}