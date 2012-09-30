using System;
using System.Collections.Generic;
using System.Linq;
using PharmaNet.Fulfillment.Application;
using PharmaNet.Fulfillment.Contract;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Fulfillment.SQL;

namespace PharmaNet.Fulfillment.Presentation
{
    public class FulfillmentService : IFulfillmentService
    {
        private PickListService _pickListService;

        public FulfillmentService()
        {
            FulfillmentDB.Initialize();

            FulfillmentDB context = new FulfillmentDB();

            _pickListService = new PickListService(context.GetPickListRepository());

            OrderHandler.Instance.Start();
        }

        public void PlaceOrder(Order order)
        {
            MessageQueue<Order>.Instance
                .Send(order);
        }

        public Confirmation CheckOrderStatus(Guid orderId)
        {
            List<PickList> pickLists = _pickListService
                .GetPickLists(orderId);

            if (!pickLists.Any())
                return null;

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
