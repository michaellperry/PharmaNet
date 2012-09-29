using System.Runtime.Serialization;

namespace PharmaNet.Fulfillment.Contract
{
    [DataContract]
    public class Shipment
    {
        [DataMember]
        public string TrackingNumber { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }
}
