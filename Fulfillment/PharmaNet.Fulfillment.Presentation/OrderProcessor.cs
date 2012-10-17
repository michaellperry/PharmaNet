using System;
using System.Threading;
using System.Transactions;
using PharmaNet.Fulfillment.Contract;
using PharmaNet.Fulfillment.SQL;
using PharmaNet.Infrastructure.Messaging;

namespace PharmaNet.Fulfillment.Presentation
{
    public class OrderProcessor
    {
        private static OrderProcessor _instance =
            new OrderProcessor();

        public static OrderProcessor Instance
        {
            get { return _instance; }
        }

        private IMessageQueueInbound<Order> _messageQueue;

        private ManualResetEvent _stop =
            new ManualResetEvent(false);
        private Thread _thread;

        public OrderProcessor()
        {
            // TODO: Inject these dependencies.
            FulfillmentDB.Initialize();

            _messageQueue = new MsmqMessageQueueInbound<Order>();

            _thread = new Thread(ThreadProc);
            _thread.Name = "OrderHandler";
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
            Order order;
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
                        if (_messageQueue.TryReceive(out order))
                        {
                            using (OrderHandler handler =
                                new OrderHandler())
                            {
                                handler.HandleOrder(order);
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