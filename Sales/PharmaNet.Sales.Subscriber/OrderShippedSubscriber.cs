using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PharmaNet.Fulfillment.Messages;
using PharmaNet.Infrastructure.Messaging;

namespace PharmaNet.Sales.Subscriber
{
    public class OrderShippedSubscriber :
        IMessageHandler<OrderShipped>
    {
        public void HandleMessage(OrderShipped message)
        {
            Console.WriteLine("Order shipped.");
        }

        public void Dispose()
        {
        }
    }
}
