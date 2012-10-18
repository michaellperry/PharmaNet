using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PharmaNet.Infrastructure.Messaging;
using PharmaNet.Fulfillment.Messages;

namespace PharmaNet.Sales.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting order shipped subscriber...");

            MessageProcessor<OrderShipped> processor =
                new MessageProcessor<OrderShipped>(
                    typeof(OrderShippedSubscriber).FullName,
                    () => new OrderShippedSubscriber());
            processor.Start();

            Console.ReadKey();

            processor.Stop();
        }
    }
}
