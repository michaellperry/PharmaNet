using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Fulfillment.Domain
{
    public class Shipment
    {
        public virtual int Id { get; set; }
        public virtual PickList PickList { get; set; }
        public virtual DateTime DateShipped { get; set; }
        public virtual string TrackingNumber { get; set; }
    }
}
