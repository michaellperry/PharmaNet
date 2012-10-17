using System;
using System.Collections.Generic;
using System.Linq;
using PharmaNet.Fulfillment.Application;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Fulfillment.Messages;
using PharmaNet.Fulfillment.SQL;

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

        public PlaceOrderHandler()
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
        }

        public void HandleMessage(PlaceOrder message)
        {
            if (_pickListService.GetPickLists(message.OrderId)
                .Any())
                return;

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

            if (_databaseError.Next(100) < 20)
                throw new ApplicationException(
                    "Database error.");

            _pickListService.SavePickLists(pickLists);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
