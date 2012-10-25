using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaNet.Fulfillment.SQL;
using PharmaNet.Fulfillment.Messages;
using PharmaNet.Infrastructure.Messaging;

namespace PharmaNet.Fulfillment.Handler
{
    class Program
    {
        static void Main(string[] args)
        {
            FulfillmentDB.Initialize();
            FulfillmentDB context = new FulfillmentDB();
            var product = EnsureProduct(context);
            EnsureWarehouse(context, product);

            Console.WriteLine("Starting order processor...");

            SubscriberRegistry<OrderShipped> subscrptionRegistry =
                new SubscriberRegistry<OrderShipped>();

            MessageProcessor<PlaceOrder> orderProcessor =
                new MessageProcessor<PlaceOrder>(
                    typeof(PlaceOrder).FullName,
                    () => new PlaceOrderHandler(subscrptionRegistry));
            orderProcessor.Start();

            MessageProcessor<Subscription> subscriptionProcessor =
                new MessageProcessor<Subscription>(
                    typeof(OrderShipped).FullName,
                    () => new SubscriptionHandler<OrderShipped>(
                        subscrptionRegistry));
            subscriptionProcessor.Start();

            Console.ReadKey();

            orderProcessor.Stop();
            subscriptionProcessor.Stop();
        }

        private static Domain.Product EnsureProduct(FulfillmentDB context)
        {
            var products = context.GetProductRepository();
            var product = products.GetAll()
                .FirstOrDefault(p => p.ProductNumber == 11190);

            if (product == null)
            {
                product = new Domain.Product { ProductNumber = 11190 };
                products.Add(product);
                products.SaveChanges();
            }

            return product;
        }

        private static void EnsureWarehouse(FulfillmentDB context, Domain.Product product)
        {
            var warehouses = context.GetWarehouseRepository();
            var warehouse = warehouses.GetAll()
                .FirstOrDefault();

            if (warehouse == null)
            {
                warehouse = new Domain.Warehouse { Name = "Hangar 18" };
                warehouse.Requisitions = new List<Domain.Requisition>()
                {
                    new Domain.Requisition
                    {
                        Product = product,
                        Quantity = 50,
                        Restocks = new List<Domain.Restock> { new Domain.Restock() }
                    }
                };
                warehouses.Add(warehouse);
                warehouses.SaveChanges();
            }
        }
    }
}
