using System.ServiceModel;
using System;

namespace PharmaNet.Fulfillment.Contract
{
    [ServiceContract]
    public interface IFulfillmentService
    {
        [OperationContract]
        Confirmation PlaceOrder(Order composite);
    }
}
