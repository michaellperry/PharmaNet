using System;
using System.Threading;
using System.Transactions;
using PharmaNet.Infrastructure.Messaging;

namespace PharmaNet.Infrastructure.Messaging
{
    public class MessageProcessor<T>
    {
        private Func<IMessageHandler<T>> _factory;
        private IMessageQueueInbound<T> _messageQueue;

        private ManualResetEvent _stop =
            new ManualResetEvent(false);
        private Thread _thread;

        public MessageProcessor(
            string queueName,
            Func<IMessageHandler<T>> factory)
        {
            _factory = factory;
            _messageQueue =
                new MsmqMessageQueueInbound<T>(
                    queueName);

            _thread = new Thread(ThreadProc);
            _thread.Name = GetType().FullName;
        }

        public void Start()
        {
            if (_thread.ThreadState == ThreadState.Unstarted)
            {
                _thread.Start();
            }
        }

        public void Stop()
        {
            _stop.Set();
            _thread.Join();
        }

        private void ThreadProc(object o)
        {
            T message;
            while (!_stop.WaitOne(0))
            {
                try
                {
                    using (var scope = new TransactionScope(
                        TransactionScopeOption.RequiresNew,
                        new TransactionOptions
                        {
                            IsolationLevel = IsolationLevel
                                .ReadCommitted
                        }))
                    {
                        if (_messageQueue.TryReceive(out message))
                        {
                            using (IMessageHandler<T> handler =
                                _factory())
                            {
                                handler.HandleMessage(message);
                            }

                            scope.Complete();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Retry.
                }
            }
        }
    }
}