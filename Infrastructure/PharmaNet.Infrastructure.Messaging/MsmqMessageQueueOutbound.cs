using System;
using System.Messaging;
using System.Transactions;

namespace PharmaNet.Infrastructure.Messaging
{
    public class MsmqMessageQueueOutbound<T> :
        IMessageQueueOutbound<T>
    {
        private XmlMessageFormatter Formatter =
            new XmlMessageFormatter(
                new Type[] { typeof(T) });

        private string _path;

        public MsmqMessageQueueOutbound(
            string target, string queueName)
        {
            _path = target + @"\private$\" + queueName;
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
                    queue.Send(message,
                        MessageQueueTransactionType.Automatic);
                }
                scope.Complete();
            }
        }
    }
}
