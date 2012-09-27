using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PharmaNet.Fulfillment.Presentation
{
    [DataContract]
    public class Order
    {
        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public bool CustomerAddress { get; set; }

        [DataMember]
        public List<Line> Lines { get; set; }
    }
}
