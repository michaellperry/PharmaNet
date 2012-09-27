using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Fulfillment.Domain
{
    public class PickList
    {
        public Warehouse Warehouse { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
