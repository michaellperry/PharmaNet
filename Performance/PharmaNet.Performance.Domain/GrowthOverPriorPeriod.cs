using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Performance.Domain
{
    public class GrowthOverPriorPeriod : Method
    {
        public override decimal CalculatePercent(SalesHistory salesHistory)
        {
            throw new NotImplementedException();
        }

        public override int CalculateUnits(SalesHistory salesHistory, decimal threshold)
        {
            throw new NotImplementedException();
        }
    }
}
