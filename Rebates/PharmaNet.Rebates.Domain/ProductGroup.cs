using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Rebates.Domain
{
    public class ProductGroup
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
