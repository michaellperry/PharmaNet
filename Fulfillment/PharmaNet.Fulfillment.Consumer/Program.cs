using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PharmaNet.Fulfillment.Contract;
using PharmaNet.Infrastructure.Service;
using System.ServiceModel.MsmqIntegration;

namespace PharmaNet.Fulfillment.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Guid> orderIds = new List<Guid>();

            ServiceClient<IFulfillmentQueryService> queryClient =
                new ServiceClient<IFulfillmentQueryService>();
            ServiceClient<IFulfillmentCommandService> commandClient =
                new ServiceClient<IFulfillmentCommandService>();
            var order = new Order
            {
                CustomerName = "Sherlock Holmes",
                CustomerAddress = "121B Baker Street",
                Lines = new List<Line>
                {
                    new Line
                    {
                        ProductNumber = 11190,
                        Quantity = 2
                    }
                }
            };

            while (true)
            {
                try
                {
                    order.OrderId = Guid.NewGuid();
                    PlaceOrder(commandClient, order);
                    orderIds.Add(order.OrderId);

                    var processedOrderIds = orderIds
                        .Where(id => CheckOrderStatus(
                            queryClient, id))
                        .ToList();
                    orderIds.RemoveAll(id =>
                        processedOrderIds.Contains(id));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void PlaceOrder(
            ServiceClient<IFulfillmentCommandService> client,
            Order order)
        {
            client.CallService(
                "MsmqEndpoint_Fulfillment",
                s => s.PlaceOrder(new MsmqMessage<Order>(order)));
        }

        private static bool CheckOrderStatus(
            ServiceClient<IFulfillmentQueryService> client,
            Guid orderId)
        {
            var confirmation = client.CallService(
                "BasicHttpBinding_IFulfillmentService",
                s => s.CheckOrderStatus(orderId));

            if (confirmation == null)
                return false;

            PrintConfirmation(confirmation);
            return true;
        }

        private static void PrintConfirmation(
            Confirmation confirmation)
        {
            String.Format("Confirmed {0} shipments:",
                confirmation.Shipments.Count);

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
