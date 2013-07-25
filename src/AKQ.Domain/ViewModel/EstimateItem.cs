namespace AKQ.Domain.ViewModel
{
    public class EstimateItem
    {
        private int _count = 0;

        public string Count { get { return Editable || Estimated ? "" : _count.ToString(); } }
        public bool Editable { get; set; }
        public bool Estimated { get; set; }
        public string Position { get; set; }

        public void Inc()
        {
            _count++;
        }
    }
}