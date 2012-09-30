using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PharmaNet.Fulfillment.Contract;
using PharmaNet.Infrastructure.Service;

namespace PharmaNet.Fulfillment.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceClient<IFulfillmentService> client = new ServiceClient<IFulfillmentService>();
            var order = new Order
            {
                CustomerName = "Sherlock Holmes",
                CustomerAddress = "121B Baker Street",
                Lines = new List<Line>
                {
                    new Line { ProductNumber = 11190, Quantity = 2 }
                }
            };

            while (true)
            {
                try
                {
                    PlaceOrder(client, order);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void PlaceOrder(ServiceClient<IFulfillmentService> client, Order order)
        {
            var confirmation = client.CallService("BasicHttpBinding_IFulfillmentService",
                            s => s.PlaceOrder(order));

            String.Format("Confirmed {0} shipments:", confirmation.Shipments.Count);

            foreach (var shipment in confirmation.Shipments)
            {
                Console.WriteLine(String.Format("{0} {1}: {2}", shipment.Quantity, shipment.ProductId, shipment.TrackingNumber));
            }
        }
    }
}
