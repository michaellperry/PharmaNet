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
                else
                {
                    orderLine.OutOfStocks.Add(new OutOfStock());
                }
            }
            _warehouseRepository.SaveChanges();
            return pickLists;
        }

        private Warehouse LocateProduct(
            Product product,
            int quantity)
        {
            var inventory = _warehouseRepository.GetAll()
                .Select(w => new
                {
                    Warehouse = w,
                    QuantityOnHand =
                        (w.Requisitions
                            .Where(r => r.Product.Id == product.Id)
                            .Where(r => r.Restocks.Any())
                            .Sum(r => (int?)r.Quantity) ?? 0) -
                        (w.PickLists
                            .Where(p => p.Product.Id == product.Id)
                            .Sum(p => (int?)p.Quantity) ?? 0)
                });
            return inventory
                .Where(q => q.QuantityOnHand >= quantity)
                .Select(q => q.Warehouse)
                .FirstOrDefault();
        }

        private PickList PickProduct(
            Guid orderId,
            Product product,
            int quantity,
            Warehouse warehouse)
        {
            PickList pickList = new PickList
            {
                OrderId = orderId,
                Product = product,
                Quantity = quantity,
                Warehouse = warehouse
            };
            warehouse.PickLists.Add(pickList);
            return pickList;
        }
    }
}
