using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Fulfillment.Messages
{
    public class PlaceOrder
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public List<Line> Lines { get; set; }
    }
}
