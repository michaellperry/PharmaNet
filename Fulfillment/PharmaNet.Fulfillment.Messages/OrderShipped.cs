using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Fulfillment.Messages
{
    public class OrderShipped
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public List<Shipment> Shipments { get; set; }
    }
}
