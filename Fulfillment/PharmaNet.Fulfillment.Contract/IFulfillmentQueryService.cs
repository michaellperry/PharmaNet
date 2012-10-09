using System.ServiceModel;
using System;

namespace PharmaNet.Fulfillment.Contract
{
    [ServiceContract(Namespace = "http://qedcode.com/PharmaNet/Fulfillment/1.0")]
    public interface IFulfillmentQueryService
    {
        [OperationContract]
        Confirmation CheckOrderStatus(Guid orderId);
    }
}
