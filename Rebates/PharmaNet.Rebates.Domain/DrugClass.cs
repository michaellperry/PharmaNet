using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Rebates.Domain
{
    public class DrugClass
    {
        public string Name { get; set; }
        public List<ProductGroup> ProductGroups { get; set; }
    }
}
