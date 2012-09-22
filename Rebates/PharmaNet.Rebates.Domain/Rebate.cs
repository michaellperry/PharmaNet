using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Rebates.Domain
{
    public class Rebate
    {
        public Method Method { get; set; }
        public ProductGroup ProductGroup { get; set; }
        public string Name { get; set; }
        public List<Tier> Tiers { get; set; }
    }
}
