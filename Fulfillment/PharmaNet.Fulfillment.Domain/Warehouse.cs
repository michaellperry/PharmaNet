using System.Collections.Generic;
using System;
using System.Linq;

namespace PharmaNet.Fulfillment.Domain
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IList<Inventory> Inventory { get; set; }
    }
}
