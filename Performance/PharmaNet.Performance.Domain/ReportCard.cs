using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Performance.Domain
{
    public class ReportCard
    {
        private Member _member;
        private Rebate _rebate;
        private MeasurementPeriod _measurementPeriod;

        private decimal _totalSalesUnits;
        private Award _award;
        private decimal _unitsToNextLevel;
    }
}
