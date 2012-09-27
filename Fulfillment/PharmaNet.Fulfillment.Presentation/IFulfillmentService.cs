using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace PharmaNet.Fulfillment.Presentation
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
