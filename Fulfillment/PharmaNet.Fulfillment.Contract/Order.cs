using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

namespace PharmaNet.Fulfillment.Contract
{
    [DataContract]
    public class Order
    {
        [DataMember]
        public Guid OrderId { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string CustomerAddress { get; set; }

        [DataMember]
        public List<Line> Lines { get; set; }
    }
}
