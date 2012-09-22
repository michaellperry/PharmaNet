using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Rebates.Domain
{
    public class Tier
    {
        public decimal Threshold { get; set; }
        public decimal PercentRemittance { get; set; }
    }
}
