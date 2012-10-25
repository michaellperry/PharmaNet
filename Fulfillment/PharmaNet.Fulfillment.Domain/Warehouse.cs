using System.Collections.Generic;

namespace PharmaNet.Fulfillment.Domain
{
    public class Warehouse
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<PickList> PickLists { get; set; }
        public virtual IList<Requisition> Requisitions { get; set; }
    }
}
