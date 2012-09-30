using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Infrastructure.Repository;

namespace PharmaNet.Fulfillment.Application
{
    public class InventoryAllocationService
    {
        private IRepository<Warehouse> _warehouseRepository;

        public InventoryAllocationService(IRepository<Warehouse> warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public List<PickList> AllocateInventory(List<OrderLine> orderLines)
        {
            List<PickList> pickLists = new List<PickList>();
            foreach (var orderLine in orderLines)
            {
                Warehouse warehouse = LocateProduct(
                    orderLine.Product,
                    orderLine.Quantity);

                if (warehouse != null)
                {
                    PickList picklist = PickProduct(
                        orderLine.Product, 
                        orderLine.Quantity, 
                        warehouse);
                    pickLists.Add(picklist);
                }
            }
            return pickLists;
        }

        private Warehouse LocateProduct(
            Product product,
            int quantity)
        {
            return _warehouseRepository.GetAll()
                .Where(warehouse => warehouse.Inventory
                    .Any(i => i.Product == product && i.QuantityOnHand >= quantity))
                .FirstOrDefault();
        }

        private PickList PickProduct(
            Product product,
            int quantity,
            Warehouse warehouse)
        {
            int inventoryOnHand = warehouse.Inventory
                .Where(i => i.Product == product)
                .Select(i => i.QuantityOnHand)
                .FirstOrDefault();

            var inventory = warehouse.Inventory
                .FirstOrDefault(i => i.Product == product);
            if (inventory == null)
            {
                warehouse.Inventory.Add(new Inventory { Product = product, QuantityOnHand = inventoryOnHand - quantity });
            }
            else
            {
                inventory.QuantityOnHand = inventoryOnHand - quantity;
            }

            return new PickList
            {
                Product = product,
                Quantity = quantity,
                Warehouse = warehouse
            };
        }
    }
}
