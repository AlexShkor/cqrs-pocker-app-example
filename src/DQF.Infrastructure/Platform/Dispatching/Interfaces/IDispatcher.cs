using System;

namespace PAQK.Platform.Dispatching.Interfaces
{
    public interface IDispatcher
    {
        void Dispatch(Object message);
        void Dispatch(Object message, Action<Exception> exceptionObserver);
    }
}