using System.ServiceModel;

namespace PharmaNet.Fulfillment.Contract
{
    [ServiceContract]
    public interface IFulfillmentService
    {
        // BAD CODE!
        // This is an example of how NOT to write a service.
        [OperationContract]
        Confirmation PlaceOrder(Order composite);
    }
}
