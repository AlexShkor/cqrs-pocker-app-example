namespace Poker.Platform.Dispatching.Interfaces
{
    public interface IMessageHandlerInterceptor
    {
        void Intercept(DispatcherInvocationContext context);
    }
}
