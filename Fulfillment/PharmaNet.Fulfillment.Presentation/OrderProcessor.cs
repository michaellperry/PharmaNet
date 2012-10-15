using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PharmaNet.Fulfillment.Application;
using PharmaNet.Fulfillment.Contract;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Fulfillment.SQL;
using System.Transactions;

namespace PharmaNet.Fulfillment.Presentation
{
    public class OrderProcessor : IDisposable
    {
        private FulfillmentDB _context;
        private CustomerService _customerService;
        private ProductService _productService;
        private InventoryAllocationService _inventoryAllocationService;
        private PickListService _pickListService;

        private Random _databaseError = new Random();

        public OrderProcessor()
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

        public void ProcessOrder(Order order)
        {
            if (_pickListService.GetPickLists(order.OrderId)
                .Any())
                return;

            Customer customer = _customerService
                .GetCustomer(
                    order.CustomerName,
                    order.CustomerAddress);

            List<OrderLine> orderLines = order.Lines
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
                        order.OrderId,
                        orderLines);

            _pickListService.SavePickLists(pickLists);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
