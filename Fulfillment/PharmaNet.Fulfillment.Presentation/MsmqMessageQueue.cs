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
            new XmlMessageFormatter(new Type[] { typeof(T) });

        private static MsmqMessageQueue<T> _instance =
            new MsmqMessageQueue<T>();

        private string _path;

        public static IMessageQueue<T> Instance
        {
            get { return _instance; }
        }

        public MsmqMessageQueue()
        {
            _path = @".\private$\" +
                typeof(T).FullName;
            if (!MessageQueue.Exists(_path))
            {
                MessageQueue.Create(
                    _path,
                    transactional: true);
            }
        }

        public void Send(T message)
        {
            using (var scope = new TransactionScope())
            {
                using (var queue = new MessageQueue(_path))
                {
                    queue.DefaultPropertiesToSend
                        .Recoverable = true;
                    queue.Formatter = Formatter;
                    queue.Send(
                        message,
                        MessageQueueTransactionType.Automatic);
                }
                scope.Complete();
            }
        }

        public bool TryReceive(out T message)
        {
            try
            {
                using (var queue = new MessageQueue(_path))
                {
                    Message queueMessage = queue.Receive(
                        Timeout,
                        MessageQueueTransactionType.Automatic);
                    queueMessage.Formatter = Formatter;
                    message = (T)queueMessage.Body;
                }
                return true;
            }
            catch (Exception x)
            {
                message = default(T);
                return false;
            }
        }
    }
}
