namespace Poker.Platform.Output
{
    public interface IInputReader
    {
        string ReadLine();
        void ReadKey();
    }
}