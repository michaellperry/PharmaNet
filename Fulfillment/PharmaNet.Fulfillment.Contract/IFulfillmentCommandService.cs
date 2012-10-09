using System.ServiceModel;
using System;
using System.ServiceModel.MsmqIntegration;

namespace PharmaNet.Fulfillment.Contract
{
    [ServiceContract(Namespace="http://qedcode.com/PharmaNet/Fulfillment/1.0")]
    [ServiceKnownType(typeof(Order))]
    public interface IFulfillmentCommandService
    {
        [OperationContract(IsOneWay=true, Action="*")]
        void PlaceOrder(MsmqMessage<Order> message);
    }
}
