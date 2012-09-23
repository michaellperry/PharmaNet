using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Rebates.Domain
{
    public class MeasuredProductGroup
    {
        public MeasuredDrugClass MeasuredDrugClass { get; set; }
        public string Name { get; set; }
        public List<MeasuredProduct> Products { get; set; }
    }
}
