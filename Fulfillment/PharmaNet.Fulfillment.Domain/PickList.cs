using System;

namespace PharmaNet.Fulfillment.Domain
{
    public class PickList
    {
        public virtual int PicklistId { get; set; }
        public virtual Guid OrderId { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Product Product { get; set; }
        public virtual int Quantity { get; set; }
    }
}
