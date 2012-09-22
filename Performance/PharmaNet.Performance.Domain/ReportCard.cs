using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Performance.Domain
{
    public class ReportCard
    {
        public Member Member { get; set; }
        public Rebate Rebate { get; set; }
        public MeasurementPeriod MeasurementPeriod { get; set; }
        public decimal TotalSalesUnits { get; set; }
        public Award Award { get; set; }
        public decimal UnitsToNextLevel { get; set; }
    }
}
