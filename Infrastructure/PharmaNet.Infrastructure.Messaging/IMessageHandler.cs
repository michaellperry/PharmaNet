using System;

namespace PharmaNet.Infrastructure.Messaging
{
    public interface IMessageHandler<T> : IDisposable
    {
        void HandleMessage(T message);
    }
}
