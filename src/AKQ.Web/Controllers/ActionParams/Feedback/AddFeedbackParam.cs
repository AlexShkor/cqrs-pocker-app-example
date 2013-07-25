using System.ComponentModel.DataAnnotations;

namespace AKQ.Web.Controllers.ActionParams.Feedback
{
    public class AddFeedbackParam
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Comment { get; set; }

        public string EmailAddress { get; set; }
    }
}