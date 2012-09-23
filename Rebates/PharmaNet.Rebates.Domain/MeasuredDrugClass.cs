using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Rebates.Domain
{
    public class MeasuredDrugClass
    {
        public string Name { get; set; }
        public List<MeasuredProductGroup> MeasuredProductGroups { get; set; }
    }
}
