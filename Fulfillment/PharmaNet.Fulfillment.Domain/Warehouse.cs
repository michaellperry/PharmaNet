using System.Collections.Generic;
using System;
using System.Linq;

namespace PharmaNet.Fulfillment.Domain
{
    public class Warehouse
    {
        public int Id { get; set; }
        public List<Inventory> Inventory { get; set; }

        public int GetInventoryOnHand(Product product)
        {
            return Inventory
                .Where(i => i.Product == product)
                .Select(i => i.QuantityOnHand)
                .FirstOrDefault();
        }

        public void SetInventoryOnHand(Product product, int value)
        {
            var inventory = Inventory
                .FirstOrDefault(i => i.Product == product);
            if (inventory == null)
            {
                Inventory.Add(new Inventory { Product = product, QuantityOnHand = value });
            }
            else
            {
                inventory.QuantityOnHand = value;
            }
        }
    }
}
