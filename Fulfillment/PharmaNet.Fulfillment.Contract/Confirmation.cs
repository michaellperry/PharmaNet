using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PharmaNet.Fulfillment.Contract
{
    [DataContract]
    public class Confirmation
    {
        [DataMember]
        public List<Shipment> Shipments { get; set; }
    }
}
