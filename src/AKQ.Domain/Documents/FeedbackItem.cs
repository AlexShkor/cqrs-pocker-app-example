using System;

namespace AKQ.Domain.Documents
{
    public class FeedbackItem
    {
        public string NameOfFeedbackLeaver { get; private set; }
        public string EmailOfFeedbackLeaver { get; private set; }
        public string Feedback { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Id { get; set; }
        public bool IsDeleted { get; private set; }

        protected FeedbackItem(){}

        public FeedbackItem(string name, string email, string feedback)
        {
            NameOfFeedbackLeaver = name;
            EmailOfFeedbackLeaver = email;
            Feedback = feedback;
            Timestamp = DateTime.Now;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }
    }
}