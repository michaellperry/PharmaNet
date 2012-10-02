using System;
using System.Collections.Generic;
using System.Linq;
using PharmaNet.Fulfillment.Application;
using PharmaNet.Fulfillment.Contract;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Fulfillment.SQL;
using System.Transactions;

namespace PharmaNet.Fulfillment.Presentation
{
    public class FulfillmentService : IFulfillmentService
    {
        private CustomerService _customerService;
        private ProductService _productService;
        private InventoryAllocationService _inventoryAllocationService;
        private PickListService _pickListService;

        public FulfillmentService()
        {
            FulfillmentDB.Initialize();

            FulfillmentDB context = new FulfillmentDB();

            _customerService = new CustomerService(context.GetCustomerRepository());
            _productService = new ProductService(context.GetProductRepository());
            _inventoryAllocationService = new InventoryAllocationService(context.GetWarehouseRepository());
            _pickListService = new PickListService(context.GetPickListRepository());

            OrderHandler.Instance.Start();
        }

        public Confirmation PlaceOrder(Order order)
        {
            List<PickList> pickLists = ProcessOrder(order);

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

        private List<PickList> ProcessOrder(Order order)
        {
            TransactionScope newTransactionScope = new TransactionScope();
            using (newTransactionScope)
            {
                Customer customer = _customerService.GetCustomer(
                    order.CustomerName, order.CustomerAddress);
                List<OrderLine> orderLines = order.Lines
                    .Select(line => new OrderLine
                    {
                        Customer = customer,
                        Product = _productService.GetProduct(
                            line.ProductNumber),
                        Quantity = line.Quantity
                    })
                    .ToList();
                List<PickList> pickLists =
                    _inventoryAllocationService.AllocateInventory(
                        order.OrderId,
                        orderLines);
                _pickListService.SavePickLists(pickLists);
                newTransactionScope.Complete();

                return pickLists;
            }
        }
    }
}
