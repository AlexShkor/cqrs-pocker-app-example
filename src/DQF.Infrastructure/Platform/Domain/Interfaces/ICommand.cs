namespace PAQK.Platform.Domain.Interfaces
{
    /// <summary>
    /// Domain Command interface
    /// </summary>
    public interface ICommand
    {
        ICommandMetadata Metadata { get; set; }
    }
}