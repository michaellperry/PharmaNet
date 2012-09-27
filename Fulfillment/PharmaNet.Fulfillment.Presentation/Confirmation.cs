using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PharmaNet.Fulfillment.Presentation
{
    [DataContract]
    public class Confirmation
    {
        [DataMember]
        public List<Shipment> Shipments;
    }
}
