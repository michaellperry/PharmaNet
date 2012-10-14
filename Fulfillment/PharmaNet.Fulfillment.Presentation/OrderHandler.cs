using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Fulfillment.Application;
using PharmaNet.Fulfillment.SQL;
using PharmaNet.Fulfillment.Contract;
using System.Transactions;
using System.Diagnostics;

namespace PharmaNet.Fulfillment.Presentation
{
    public class OrderHandler
    {
        private static OrderHandler _instance =
            new OrderHandler();

        public static OrderHandler Instance
        {
            get { return _instance; }
        }

        private IMessageQueue<Order> _messageQueue;

        private ManualResetEvent _stop =
            new ManualResetEvent(false);
        private Thread _thread;

        public OrderHandler()
        {
            // TODO: Inject these dependencies.
            FulfillmentDB.Initialize();

            _messageQueue = MsmqMessageQueue<Order>
                .Instance;

            _thread = new Thread(ThreadProc);
            _thread.Name = "OrderHandler";
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
                    using (var scope = new TransactionScope())
                    {
                        using (OrderProcessor processor = new OrderProcessor())
                        {
                            if (_messageQueue.TryReceive(out order))
                            {
                                processor.ProcessOrder(order);
                            }
                        }

                        scope.Complete();
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