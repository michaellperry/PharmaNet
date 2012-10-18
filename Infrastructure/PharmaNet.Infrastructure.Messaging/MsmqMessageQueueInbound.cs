using System;
using System.Messaging;
using System.Transactions;

namespace PharmaNet.Infrastructure.Messaging
{
    public class MsmqMessageQueueInbound<T> :
        IMessageQueueInbound<T>
    {
        private static readonly TimeSpan Timeout =
            TimeSpan.FromSeconds(5.0);
        private XmlMessageFormatter Formatter =
            new XmlMessageFormatter(
                new Type[] { typeof(T) });

        private string _path;

        public MsmqMessageQueueInbound(string queueName)
        {
            _path = @".\private$\" + queueName;
            if (!MessageQueue.Exists(_path))
            {
                MessageQueue.Create(_path,
                    transactional: true);
            }
        }

        public bool TryReceive(out T message)
        {
            try
            {
                using (var queue = new MessageQueue(_path))
                {
                    var msmqMessage = queue.Receive(Timeout,
                        MessageQueueTransactionType.Automatic);
                    msmqMessage.Formatter = Formatter;
                    message = (T)msmqMessage.Body;
                    return true;
                }
            }
            catch (Exception x)
            {
                message = default(T);
                return false;
            }
        }
    }
}
