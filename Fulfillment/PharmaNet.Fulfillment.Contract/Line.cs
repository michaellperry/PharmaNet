using System.Runtime.Serialization;

namespace PharmaNet.Fulfillment.Contract
{
    [DataContract]
    public class Line
    {
        [DataMember]
        public int ProductNumber { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }
}
