using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PharmaNet.Fulfillment.Application;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Infrastructure.Repository;

namespace PharmaNet.Fulfillment.Test
{
    [TestClass]
    public class InventoryAllcationServiceTest
    {
        private InventoryAllocationService _service;
        private FakeRepository<Warehouse> _warehouses;

        private Warehouse _warehouse1;
        private Product _procrit;
        private Customer _clinic;
        [TestInitialize]
        public void Initialize()
        {
            _warehouses = new FakeRepository<Warehouse>();
            _warehouse1 = new Warehouse { Name = "Hanger 18" };
            _procrit = new Product { Id = 11190 };

            _warehouse1.Requisitions = new List<Requisition>();
            _warehouse1.Requisitions.Add(new Requisition
            {
                Product = _procrit,
                Quantity = 7,
                Restocks = new List<Restock> { new Restock() }
            });
            _warehouse1.PickLists = new List<PickList>();
            _warehouses.Add(_warehouse1);

            _clinic = new Customer
            {
                Name = "The Clinic",
                ShippingAddress = "1 My Way"
            };

            _service = new InventoryAllocationService(_warehouses);
        }

        [TestMethod]
        public void CanAllocateFromOneWarehouse()
        {
            List<PickList> pickLists = _service.AllocateInventory(
                Guid.NewGuid(),
                new List<OrderLine>
                {
                    new OrderLine
                    {
                        Customer = _clinic,
                        Product = _procrit,
                        Quantity = 3
                    }
                });

            Assert.AreEqual(1, pickLists.Count);
            Assert.AreEqual(11190, pickLists[0].Product.Id);
            Assert.AreEqual(3, pickLists[0].Quantity);
        }

        [TestMethod]
        public void OutOfStock()
        {
            List<PickList> pickLists = _service.AllocateInventory(
                Guid.NewGuid(),
                new List<OrderLine>
                {
                    new OrderLine
                    {
                        Customer = _clinic,
                        Product = _procrit,
                        Quantity = 10
                    }
                });

            Assert.AreEqual(0, pickLists.Count);
        }

        [TestMethod]
        public void InventoryChangesWhenPicked()
        {
            _service.AllocateInventory(
                Guid.NewGuid(),
                new List<OrderLine>
                {
                    new OrderLine
                    {
                        Customer = _clinic,
                        Product = _procrit,
                        Quantity = 3
                    }
                });

            var warehouse = _warehouse1;
            var product = _procrit;
            int quantity =
                warehouse
                    .Requisitions
                    .Where(r => r.Product == product)
                    .Where(r => r.Restocks.Any())
                    .Sum(r => r.Quantity) -
                warehouse
                    .PickLists
                    .Where(p => p.Product == product)
                    .Sum(p => p.Quantity);
            Assert.AreEqual(4, quantity);
        }
    }
}
