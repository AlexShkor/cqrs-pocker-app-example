using System.Collections.Generic;

namespace AKQ.Web.Models
{
    public class ExplainedTrickViewModel
    {
        public int Index { get; set; }
        public List<PlayViewModel> Players { get; set; }

        public ExplainedTrickViewModel()
        {
            
        }

        public ExplainedTrickViewModel(int index)
        {
            Index = index;
            Players = new List<PlayViewModel>();
        }
    }
}