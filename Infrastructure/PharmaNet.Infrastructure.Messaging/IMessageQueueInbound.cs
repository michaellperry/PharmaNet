using System.Collections.Generic;
using System.Linq;
using System;

namespace PharmaNet.Infrastructure.Messaging
{
    public interface IMessageQueueInbound<T>
    {
        bool TryReceive(out T message);
    }
}
