using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;

namespace PharmaNet.Infrastructure.Messaging
{
    public class MemoryMessageQueue<T> :
        IMessageQueueInbound<T>,
        IMessageQueueOutbound<T>
    {
        private static readonly TimeSpan Timeout =
            TimeSpan.FromSeconds(30.0);

        private Queue<T> _messages =
            new Queue<T>();
        private ManualResetEvent _notEmpty =
            new ManualResetEvent(false);

        public void Send(T message)
        {
            lock (this)
            {
                _messages.Enqueue(message);
                _notEmpty.Set();
            }
        }

        public bool TryReceive(out T message)
        {
            bool signalled = _notEmpty
                .WaitOne(Timeout);

            if (signalled)
            {
                lock (this)
                {
                    if (_messages.Any())
                    {
                        message = _messages.Dequeue();
                        if (!_messages.Any())
                            _notEmpty.Reset();
                        return true;
                    }
                }
            }
            message = default(T);
            return false;
        }
    }
}