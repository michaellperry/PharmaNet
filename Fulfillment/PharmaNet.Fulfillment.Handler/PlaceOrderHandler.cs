using System;
using System.Collections.Generic;
using System.Linq;
using PharmaNet.Fulfillment.Application;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Fulfillment.Messages;
using PharmaNet.Fulfillment.SQL;
using PharmaNet.Infrastructure.Messaging;

namespace PharmaNet.Fulfillment.Handler
{
    public class PlaceOrderHandler :
        IMessageHandler<PlaceOrder>
    {
        private FulfillmentDB _context;
        private CustomerService _customerService;
        private ProductService _productService;
        private InventoryAllocationService _inventoryAllocationService;
        private PickListService _pickListService;

        private Random _databaseError = new Random();

        private SubscriberRegistry<OrderShipped> _subscriptionRegistry;

        public PlaceOrderHandler(
            SubscriberRegistry<OrderShipped> subscriptionRegistry)
        {
            _context = new FulfillmentDB();

            _customerService = new CustomerService(
                _context.GetCustomerRepository());
            _productService = new ProductService(
                _context.GetProductRepository());
            _inventoryAllocationService = new InventoryAllocationService(
                _context.GetWarehouseRepository());
            _pickListService = new PickListService(
                _context.GetPickListRepository());

            _subscriptionRegistry = subscriptionRegistry;
        }

        public void HandleMessage(PlaceOrder message)
        {
            if (_pickListService.GetPickLists(message.OrderId)
                .Any())
                return;

            Console.WriteLine("Place order.");
            Customer customer = _customerService
                .GetCustomer(
                    message.CustomerName,
                    message.CustomerAddress);

            List<OrderLine> orderLines = message.Lines
                .Select(line => new OrderLine
                {
                    Customer = customer,
                    Product = _productService
                        .GetProduct(
                            line.ProductNumber),
                    Quantity = line.Quantity
                })
                .ToList();

            List<PickList> pickLists =
                _inventoryAllocationService
                    .AllocateInventory(
                        message.OrderId,
                        orderLines);

            _pickListService.SavePickLists(pickLists);

            var orderShippedEvent = new OrderShipped
            {
                OrderId = message.OrderId,
                OrderDate = message.OrderDate,
                CustomerName = message.CustomerName,
                CustomerAddress = message.CustomerAddress,
                Shipments = pickLists
                    .Select(p => new Messages.Shipment
                    {
                        ProductNumber = p.Product.ProductNumber,
                        Quantity = p.Quantity,
                        TrackingNumber = "123-45"
                    })
                    .ToList()
            };
            _subscriptionRegistry.Publish(orderShippedEvent);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
