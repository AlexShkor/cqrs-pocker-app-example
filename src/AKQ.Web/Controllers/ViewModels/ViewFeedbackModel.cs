using System.Collections.Generic;
using AKQ.Domain.Documents;

namespace AKQ.Web.Controllers.ViewModels
{
    public class ViewFeedbackModel
    {
        public IEnumerable<FeedbackItem> FeedbackItems { get; set; }
    }
}