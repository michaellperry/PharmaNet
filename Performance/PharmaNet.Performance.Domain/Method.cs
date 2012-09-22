using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Performance.Domain
{
    public abstract class Method
    {
        public abstract decimal CalculatePercent(SalesHistory salesHistory);
        public abstract int CalculateUnits(SalesHistory salesHistory, decimal threshold);
    }
}
