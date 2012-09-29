using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PharmaNet.Fulfillment.Contract
{
    [DataContract]
    public class Order
    {
        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string CustomerAddress { get; set; }

        [DataMember]
        public List<Line> Lines { get; set; }
    }
}
