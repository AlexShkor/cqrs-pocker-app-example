using System;

namespace AKQ.Domain.Documents
{
    public class TournamentGameInfo
    {
        public string UserId { get; set; }
        public string GameId { get; set; }
        public string DealId { get; set; }
        public DateTime Started { get; set; }
        public DateTime? Finished { get; set; }
        public ContractDocument Contract { get; set; }
        public int? Result { get; set; }
        public string UserName { get; set; }


        public string GetFormatedResult()
        {
            return Result == 0
                       ? "="
                       : Result > 0 ? "+" + Result.ToString() : Result.ToString();
        }
    }
}