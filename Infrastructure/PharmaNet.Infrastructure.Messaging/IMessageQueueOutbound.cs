using System.Collections.Generic;
using System.Linq;
using System;

namespace PharmaNet.Infrastructure.Messaging
{
    public interface IMessageQueueOutbound<T>
    {
        void Send(T message);
    }
}
