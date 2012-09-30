using System;
using PharmaNet.Fulfillment.Contract;
using PharmaNet.Fulfillment.Application;
using PharmaNet.Fulfillment.SQL;
using System.Collections.Generic;
using PharmaNet.Fulfillment.Domain;
using System.Linq;
using PharmaNet.Infrastructure.Repository;

namespace PharmaNet.Fulfillment.Presentation
{
    public class FulfillmentService : IFulfillmentService
    {
        // BAD CODE!
        // This is an example of how NOT to write a service.
        public Confirmation PlaceOrder(Order order)
        {
            // TODO: Inject these dependencies.
            FulfillmentDB.Initialize();

            FulfillmentDB context = new FulfillmentDB();
            InventoryAllocationService inventoryAllocationService = new InventoryAllocationService(context.GetWarehouseRepository());
            IRepository<Customer> customers = context.GetCustomerRepository();
            Customer customer = customers.GetAll()
                .FirstOrDefault(c => c.Name == order.CustomerName);
            if (customer == null)
            {
                customer = customers.Add(new Customer
                {
                    Name = order.CustomerName,
                    ShippingAddress = order.CustomerAddress
                });
            }
            List<OrderLine> orderLines = order.Lines
                .Select(line => new OrderLine
                {
                    Customer = customer,
                    Product = GetProduct(context, line.ProductNumber),
                    Quantity = line.Quantity
                })
                .ToList();
            List<PickList> pickLists = inventoryAllocationService.AllocateInventory(orderLines);

            return new Confirmation
            {
                Shipments = pickLists
                    .Select(pickList => new Shipment
                    {
                        ProductId = pickList.Product.ProductId,
                        Quantity = pickList.Quantity,
                        TrackingNumber = "123-456"
                    })
                    .ToList()
            };
        }

        private Product GetProduct(FulfillmentDB context, int productNumber)
        {
            IRepository<Product> products = context.GetProductRepository();
            return products.GetAll()
                .FirstOrDefault(p => p.ProductNumber == productNumber);
        }
    }
}
