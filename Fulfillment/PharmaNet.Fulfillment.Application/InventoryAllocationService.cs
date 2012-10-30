using System;
using System.Collections.Generic;
using System.Linq;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Infrastructure.Repository;
using System.Diagnostics;

namespace PharmaNet.Fulfillment.Application
{
    public class InventoryAllocationService
    {
        private IRepository<Warehouse> _warehouseRepository;

        public InventoryAllocationService(
            IRepository<Warehouse> warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public List<PickList> AllocateInventory(
            Guid orderId,
            List<OrderLine> orderLines)
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
                        orderId,
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
            var inventoryLevels =
                from warehouse in _warehouseRepository.GetAll()
                select new
                {
                    Warehouse = warehouse,
                    QuantityAllocated = (int ?)warehouse.PickLists
                        .Where(p => p.Product.Id == product.Id)
                        .Sum(p => p.Quantity) ?? 0,
                    QuantityRestocked = (int ?)warehouse.Requisitions
                        .Where(r => r.Product.Id == product.Id)
                        .Where(r => r.Restocks.Any())
                        .Sum(r => r.Quantity) ?? 0
                };
            return inventoryLevels
                .Where(l => l.QuantityRestocked - l.QuantityAllocated >=
                    quantity)
                .Select(l => l.Warehouse)
                .FirstOrDefault();
        }

        private PickList PickProduct(
            Guid orderId,
            Product product,
            int quantity,
            Warehouse warehouse)
        {
            return new PickList
            {
                OrderId = orderId,
                Product = product,
                Quantity = quantity,
                Warehouse = warehouse
            };
        }
    }
}
