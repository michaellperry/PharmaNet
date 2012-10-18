using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Sales.Domain
{
    public class SalesHistory
    {
        public virtual int Id { get; set; }
        public virtual Member Member { get; set; }
        public virtual MeasurementPeriod MeasurementPeriod { get; set; }
        public virtual Rebate Rebate { get; set; }
        public virtual List<Sale> Sales { get; set; }
    }
}
