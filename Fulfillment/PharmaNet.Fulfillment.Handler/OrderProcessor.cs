using PharmaNet.Fulfillment.Contract;
using PharmaNet.Fulfillment.SQL;
using PharmaNet.Infrastructure.Messaging;
using System;
using System.Threading;
using System.Transactions;

namespace PharmaNet.Fulfillment.Handler
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
            _thread.Name = "OrderProcessor";
        }

        public void Start()
        {
            if (_thread.ThreadState ==
                System.Threading.ThreadState.Unstarted)
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
                                Console.WriteLine("Handle order.");
                                handler.HandleOrder(order);
                            }

                            scope.Complete();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Retry.
                    Console.WriteLine("Retry.");
                }
            }
        }
    }
}