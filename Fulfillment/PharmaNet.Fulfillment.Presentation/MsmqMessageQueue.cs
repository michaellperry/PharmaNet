using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;

namespace PharmaNet.Fulfillment.Presentation
{
    public class MsmqMessageQueue<T> : IMessageQueue<T>
    {
        private static readonly TimeSpan Timeout =
            TimeSpan.FromSeconds(30.0);

        private static MsmqMessageQueue<T> _instance =
            new MsmqMessageQueue<T>();

        private MessageQueue _queue;

        public static IMessageQueue<T> Instance
        {
            get { return _instance; }
        }

        public MsmqMessageQueue()
        {
            string name = @".\private$\" +
                typeof(T).FullName;
            if (!MessageQueue.Exists(name))
            {
                MessageQueue.Create(
                    name,
                    transactional: true);
            }
            _queue = new MessageQueue(name);
            _queue.DefaultPropertiesToSend
                .Recoverable = true;
        }

        public void Send(T message)
        {
            _queue.Send(message);
        }

        public bool TryReceive(out T message)
        {
            try
            {
                message = (T)_queue
                    .Receive(Timeout)
                    .Body;
                return true;
            }
            catch (Exception ex)
            {
                message = default(T);
                return false;
            }
        }
    }
}
