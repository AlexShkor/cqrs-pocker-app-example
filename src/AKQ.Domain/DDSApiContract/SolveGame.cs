using System.Collections.Generic;
using ServiceStack.ServiceHost;

namespace DdsContract
{

    [Route("/solve-game")]
    public class SolveGame : IReturn<SolveGameResponse>
    {
        public string PBN { get; set; }
    }

    public class SolveGameResponse
    {
        public List<TrickResult> Tricks { get; set; }

        public SolveGameResponse()
        {
            Tricks = new List<TrickResult>();
        }
    }
}