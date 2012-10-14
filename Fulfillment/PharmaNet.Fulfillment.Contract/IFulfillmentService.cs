using System.ServiceModel;
using System;

namespace PharmaNet.Fulfillment.Contract
{
    [ServiceContract]
    public interface IFulfillmentService
    {
        [OperationContract]
        [FaultContract(typeof(FulfillmentNetworkError))]
        void PlaceOrder(Order composite);

        [OperationContract]
        [FaultContract(typeof(FulfillmentNetworkError))]
        Confirmation CheckOrderStatus(Guid orderId);
    }
}
