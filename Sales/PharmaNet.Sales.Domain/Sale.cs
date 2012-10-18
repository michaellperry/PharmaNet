using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Sales.Domain
{
    public class Sale
    {
        public virtual int Id { get; set; }
        public virtual Product Product { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int Units { get; set; }
    }
}
