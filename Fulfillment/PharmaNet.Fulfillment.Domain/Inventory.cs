using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaNet.Fulfillment.Domain
{
    public class Inventory
    {
        [Key, Column(Order = 1)]
        public virtual int WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; }

        [Key, Column(Order = 2)]
        public virtual int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public virtual int QuantityOnHand { get; set; }
    }
}
