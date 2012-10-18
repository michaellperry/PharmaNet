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
            List<Guid> orderIds = new List<Guid>();

            ServiceClient<IFulfillmentService> client =
                new ServiceClient<IFulfillmentService>();
            var order = new Order
            {
                CustomerName = "Sherlock Holmes",
                CustomerAddress = "121B Baker Street",
                OrderDate = DateTime.Today,
                Lines = new List<Line>
                {
                    new Line
                    {
                        ProductNumber = 11190,
                        Quantity = 1
                    }
                }
            };

            for (int i = 0; i < 100; ++i)
            {
                Console.WriteLine("Place order {0}.", i);

                order.OrderId = Guid.NewGuid();
                PlaceOrder(client, order);
                orderIds.Add(order.OrderId);

                CheckPendingOrders(client, orderIds);
            }

            while (orderIds.Any())
                CheckPendingOrders(client, orderIds);

            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        private static void CheckPendingOrders(
            ServiceClient<IFulfillmentService> client,
            List<Guid> orderIds)
        {
            orderIds.RemoveAll(id =>
                CheckOrderStatus(client, id));

            Console.WriteLine("Pending orders: {0}.",
                orderIds.Count);
        }

        private static void PlaceOrder(
            ServiceClient<IFulfillmentService> client,
            Order order)
        {
            while (true)
            {
                try
                {
                    client.CallService(
                        "BasicHttpBinding_IFulfillmentService",
                        s => s.PlaceOrder(order));
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Network error: retry.");
                }
            }
        }

        private static bool CheckOrderStatus(
            ServiceClient<IFulfillmentService> client,
            Guid orderId)
        {
            Confirmation confirmation = null;
            try
            {
                confirmation = client.CallService(
                    "BasicHttpBinding_IFulfillmentService",
                    s => s.CheckOrderStatus(orderId));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            if (confirmation == null)
                return false;

            PrintConfirmation(confirmation);
            return true;
        }

        private static void PrintConfirmation(
            Confirmation confirmation)
        {
            foreach (var shipment in confirmation.Shipments)
            {
                Console.WriteLine(String.Format(
                    "{0} {1}: {2}",
                    shipment.Quantity,
                    shipment.ProductId,
                    shipment.TrackingNumber));
            }
        }
    }
}
