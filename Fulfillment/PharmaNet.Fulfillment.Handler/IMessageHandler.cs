using System;

namespace PharmaNet.Fulfillment.Handler
{
    public interface IMessageHandler<T> : IDisposable
    {
        void HandleMessage(T message);
    }
}
