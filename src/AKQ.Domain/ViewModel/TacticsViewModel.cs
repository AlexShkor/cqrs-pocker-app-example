using System.Collections.Generic;
using System.Linq;
using AKQ.Domain.Documents;

namespace AKQ.Domain.ViewModel
{
    public class TacticsViewModel
    {
        public List<TagViewModel> Tags { get; set; }

        public TacticsViewModel()
        {
            Tags = new List<TagViewModel>()
            {
                new TagViewModel(1,"draw trumps"),
                new TagViewModel(2,"finesse"),
                new TagViewModel(3,"establish side suit"),
                new TagViewModel(4,"communication"),
                new TagViewModel(5,"cross-ruff"),
                new TagViewModel(6,"end play"),
                new TagViewModel(7,"squeeze"),
                new TagViewModel(8,"discard losers"),
                new TagViewModel(9,"loser on loser"),
                new TagViewModel(10,"hold-up play"),
            };
        }

        public TacticsViewModel(List<Tag> selected):this()
        {
            foreach (var tag in Tags)
            {
                if (selected.Any(x=> x.Id == tag.Id))
                {
                    tag.Selected = true;
                }
            }
        }
    }
}