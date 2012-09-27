using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PharmaNet.Fulfillment.Presentation
{
    public class FulfillmentService : IFulfillmentService
    {
        // BAD CODE!
        // This is an example of how NOT to write a service.
        public Confirmation PlaceOrder(Order composite)
        {
            throw new NotImplementedException();
        }
    }
}
