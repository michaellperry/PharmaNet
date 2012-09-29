using System.Runtime.Serialization;

namespace PharmaNet.Fulfillment.Contract
{
    [DataContract]
    public class Line
    {
        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }
}
