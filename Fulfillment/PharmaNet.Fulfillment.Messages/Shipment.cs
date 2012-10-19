using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Fulfillment.Messages
{
    public class Shipment
    {
        public string TrackingNumber { get; set; }
        public int ProductNumber { get; set; }
        public int Quantity { get; set; }
    }
}
