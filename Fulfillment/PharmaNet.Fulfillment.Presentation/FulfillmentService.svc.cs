using System.Collections.Generic;
using System.Linq;
using PharmaNet.Fulfillment.Application;
using PharmaNet.Fulfillment.Contract;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Fulfillment.SQL;
using PharmaNet.Infrastructure.Repository;

namespace PharmaNet.Fulfillment.Presentation
{
    public class FulfillmentService : IFulfillmentService
    {
        private CustomerService _customerService;
        private ProductService _productService;
        private InventoryAllocationService _inventoryAllocationService;

        public FulfillmentService()
        {
            // TODO: Inject these dependencies.
            FulfillmentDB.Initialize();

            FulfillmentDB context = new FulfillmentDB();

            _customerService = new CustomerService(context.GetCustomerRepository());
            _productService = new ProductService(context.GetProductRepository());
            _inventoryAllocationService = new InventoryAllocationService(context.GetWarehouseRepository());
        }

        // BAD CODE!
        // This is an example of how NOT to write a service.
        public Confirmation PlaceOrder(Order order)
        {
            Customer customer = _customerService.GetCustomer(order.CustomerName, order.CustomerAddress);
            List<OrderLine> orderLines = order.Lines
                .Select(line => new OrderLine
                {
                    Customer = customer,
                    Product = _productService.GetProduct(line.ProductNumber),
                    Quantity = line.Quantity
                })
                .ToList();
            List<PickList> pickLists = _inventoryAllocationService.AllocateInventory(orderLines);

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
    }
}
