using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmaNet.Fulfillment.Domain
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set;  }
        public string ShippingAddress { get; set; }
    }
}
