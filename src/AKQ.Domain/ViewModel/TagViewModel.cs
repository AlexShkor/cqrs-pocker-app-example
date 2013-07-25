namespace AKQ.Domain.ViewModel
{
    public class TagViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Selected { get; set; }

        public TagViewModel()
        {
            
        }

        public TagViewModel(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}