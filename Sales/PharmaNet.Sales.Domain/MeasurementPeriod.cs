using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Sales.Domain
{
    public class MeasurementPeriod
    {
        public virtual int Id { get; set; }
        public virtual DateTime StartDate { get; set; }
    }
}
