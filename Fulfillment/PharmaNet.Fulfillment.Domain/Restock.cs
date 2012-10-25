using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Fulfillment.Domain
{
    public class Restock
    {
        public virtual int Id { get; set; }
        public virtual Requisition Requisition { get; set; }
    }
}
