using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaNet.Fulfillment.Domain
{
    public class Inventory
    {
        [Key, Column(Order = 1)]
        public int WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; }

        [Key, Column(Order = 2)]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int QuantityOnHand { get; set; }
    }
}
