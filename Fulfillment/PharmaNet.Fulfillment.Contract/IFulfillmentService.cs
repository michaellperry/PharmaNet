using System.ServiceModel;
using System;

namespace PharmaNet.Fulfillment.Contract
{
    [ServiceContract]
    public interface IFulfillmentService
    {
        [OperationContract]
        void PlaceOrder(Order composite);

        [OperationContract]
        Confirmation CheckOrderStatus(Guid orderId);
    }
}
