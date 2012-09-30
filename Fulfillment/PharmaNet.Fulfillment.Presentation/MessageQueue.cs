using System.Collections.Generic;
using System.Linq;

namespace PharmaNet.Fulfillment.Presentation
{
    public class MessageQueue<T>
    {
        private static MessageQueue<T> _instance =
            new MessageQueue<T>();

        public static MessageQueue<T> Instance
        {
            get { return _instance; }
        }

        private Queue<T> _messages =
            new Queue<T>();

        public void Send(T message)
        {
            lock (this)
            {
                _messages.Enqueue(message);
            }
        }

        public bool TryReceive(out T message)
        {
            lock (this)
            {
                if (_messages.Any())
                {
                    message = _messages.Dequeue();
                    return true;
                }
                else
                {
                    message = default(T);
                    return false;
                }
            }
        }
    }
}