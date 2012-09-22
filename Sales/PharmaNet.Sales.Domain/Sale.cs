using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Sales.Domain
{
    public class Sale
    {
        public Participant Participant { get; set; }
        public MeasurementPeriod MeasurementPeriod { get; set; }
        public Product Product { get; set; }

        public DateTime Date { get; set; }
        public int Units { get; set; }
    }
}
