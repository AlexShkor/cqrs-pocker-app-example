namespace AKQ.Domain.ViewModel
{
    public class TrickViewModel
    {
        public string LedSuit { get; private set; }
        public TrickItemViewModel W { get; private set; }
        public TrickItemViewModel N { get; private set; }
        public TrickItemViewModel E { get; private set; }
        public TrickItemViewModel S { get; private set; }
        public bool Completed { get; private set; }

        public TrickViewModel(Trick trick)
        {
            LedSuit = trick.IsEmpty ? null : trick.LedSuit.ToShortName();
            W = new TrickItemViewModel(trick.GetCard(PlayerPosition.West));
            N = new TrickItemViewModel(trick.GetCard(PlayerPosition.North));
            E = new TrickItemViewModel(trick.GetCard(PlayerPosition.East));
            S = new TrickItemViewModel(trick.GetCard(PlayerPosition.South));
            Completed = trick.TrickIsComplete;
        }
    }
}