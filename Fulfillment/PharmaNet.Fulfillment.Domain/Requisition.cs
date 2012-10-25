using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Fulfillment.Domain
{
    public class Requisition
    {
        public virtual int Id { get; set; }
        public virtual Product Product { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual int Quantity { get; set; }
        public virtual IList<Restock> Restocks { get; set; }
    }
}
