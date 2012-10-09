using System;
using System.Linq;
using PharmaNet.Fulfillment.Application;
using PharmaNet.Fulfillment.Contract;
using PharmaNet.Fulfillment.SQL;

namespace PharmaNet.Fulfillment.Presentation
{
    public class FulfillmentQueryService : IFulfillmentQueryService
    {
        private CustomerService _customerService;
        private ProductService _productService;
        private InventoryAllocationService _inventoryAllocationService;
        private PickListService _pickListService;

        public FulfillmentQueryService()
        {
            FulfillmentDB.Initialize();

            FulfillmentDB context = new FulfillmentDB();

            _customerService = new CustomerService(
                context.GetCustomerRepository());
            _productService = new ProductService(
                context.GetProductRepository());
            _inventoryAllocationService = new InventoryAllocationService(
                context.GetWarehouseRepository());
            _pickListService = new PickListService(
                context.GetPickListRepository());
        }

        public Confirmation CheckOrderStatus(Guid orderId)
        {
            var pickLists = _pickListService
                .GetPickLists(orderId);

            if (!pickLists.Any())
                return null;

            return new Confirmation
            {
                Shipments = pickLists
                    .Select(pickList => new Shipment
                    {
                        ProductId = pickList.Product
                            .ProductId,
                        Quantity = pickList.Quantity,
                        TrackingNumber = "123-456"
                    })
                    .ToList()
            };
        }
    }
}
