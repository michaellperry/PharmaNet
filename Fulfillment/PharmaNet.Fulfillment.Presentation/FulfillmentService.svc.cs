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
        private IRepository<Warehouse> _warehouseRepository;
        private IRepository<Customer> _customerRepository;
        private IRepository<Product> _productRepository;

        public FulfillmentService()
        {
            // TODO: Inject these dependencies.
            FulfillmentDB.Initialize();

            FulfillmentDB context = new FulfillmentDB();

            _warehouseRepository = context.GetWarehouseRepository();
            _customerRepository = context.GetCustomerRepository();
            _productRepository = context.GetProductRepository();
        }

        // BAD CODE!
        // This is an example of how NOT to write a service.
        public Confirmation PlaceOrder(Order order)
        {
            InventoryAllocationService inventoryAllocationService = new InventoryAllocationService(_warehouseRepository);
            Customer customer = GetCustomer(order);
            List<OrderLine> orderLines = order.Lines
                .Select(line => new OrderLine
                {
                    Customer = customer,
                    Product = GetProduct(line.ProductNumber),
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

        private Customer GetCustomer(Order order)
        {
            Customer customer = _customerRepository.GetAll()
                .FirstOrDefault(c => c.Name == order.CustomerName);
            if (customer == null)
            {
                customer = _customerRepository.Add(new Customer
                {
                    Name = order.CustomerName,
                    ShippingAddress = order.CustomerAddress
                });
                _customerRepository.SaveChanges();
            }
            return customer;
        }

        private Product GetProduct(int productNumber)
        {
            return _productRepository.GetAll()
                .FirstOrDefault(p => p.ProductNumber == productNumber);
        }
    }
}
