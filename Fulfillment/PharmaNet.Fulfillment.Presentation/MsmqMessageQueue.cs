using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Transactions;
using System.Diagnostics;
using System.Runtime.Serialization;

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
            string path = @".\private$\" +
                typeof(T).FullName;
            if (!MessageQueue.Exists(path))
            {
                MessageQueue.Create(
                    path,
                    transactional: true);
            }
            _queue = new MessageQueue(path);
            _queue.DefaultPropertiesToSend
                .Recoverable = true;
            _queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(T) });
        }

        public void Send(T message)
        {
            using (var scope = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                }
            ))
            {
                _queue.Send(message,
                    MessageQueueTransactionType.Automatic);
                scope.Complete();
            }
        }

        public bool TryReceive(out T message)
        {
            try
            {
                Debug.WriteLine("Messages in queue: {0}.", _queue.GetAllMessages().Length);
                message = (T)_queue.Receive(Timeout,
                    MessageQueueTransactionType.Automatic)
                    .Body;
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
