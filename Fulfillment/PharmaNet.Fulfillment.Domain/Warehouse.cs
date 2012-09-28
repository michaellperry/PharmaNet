using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmaNet.Fulfillment.Domain
{
    public class Warehouse
    {
        private Dictionary<int, int> _inventoryOnHandByProductId = new Dictionary<int, int>();

        public int GetInventoryOnHand(Product product)
        {
            int inventoryOnHand;
            if (!_inventoryOnHandByProductId.TryGetValue(product.ProductId, out inventoryOnHand))
                return 0;
            return inventoryOnHand;
        }

        public void SetInventoryOnHand(Product product, int value)
        {
            _inventoryOnHandByProductId[product.ProductId] = value;
        }
    }
}
