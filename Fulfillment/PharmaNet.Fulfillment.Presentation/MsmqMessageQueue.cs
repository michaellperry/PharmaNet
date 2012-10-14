using System;
using System.Messaging;
using System.Transactions;

namespace PharmaNet.Fulfillment.Presentation
{
    public class MsmqMessageQueue<T> : IMessageQueue<T>
    {
        private static readonly TimeSpan Timeout =
            TimeSpan.FromSeconds(30.0);
        private XmlMessageFormatter Formatter =
            new XmlMessageFormatter(
                new Type[] { typeof(T) });

        private static MsmqMessageQueue<T> _instance =
            new MsmqMessageQueue<T>();

        private string _path;

        public static IMessageQueue<T> Instance
        {
            get { return _instance; }
        }

        public MsmqMessageQueue()
        {
        }

        public void Send(T message)
        {
        }

        public bool TryReceive(out T message)
        {
            message = default(T);
            return false;
        }
    }
}
