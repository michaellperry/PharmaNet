using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Sales.Domain
{
    public class Product
    {
        public virtual int Id { get; set; }
        public virtual Rebate Rebate { get; set; }
        public virtual int ProductNumber { get; set; }
        public virtual string Name { get; set; }
    }
}
