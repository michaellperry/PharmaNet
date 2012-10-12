using System.Collections.Generic;
using System.Linq;
using System;

namespace PharmaNet.Fulfillment.Presentation
{
    public interface IMessageQueue<T>
    {
        void Send(T message);
        bool TryReceive(out T message);
    }
}
