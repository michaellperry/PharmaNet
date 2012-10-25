using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Fulfillment.Domain
{
    public class OutOfStock
    {
        public virtual int Id { get; set; }
        public virtual OrderLine OrderLine { get; set; }
    }
}
