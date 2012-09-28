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
            _warehouse1 = new Warehouse();
            _procrit = new Product { ProductId = 11190 };
            _warehouse1.SetInventoryOnHand(_procrit, 7);
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
            List<PickList> pickLists = _service.AllocateInventory(new List<OrderLine>
            {
                new OrderLine
                {
                    Customer = _clinic,
                    Product = _procrit,
                    Quantity = 3
                }
            });

            Assert.AreEqual(1, pickLists.Count);
            Assert.AreEqual(11190, pickLists[0].Product.ProductId);
            Assert.AreEqual(3, pickLists[0].Quantity);
        }

        [TestMethod]
        public void OutOfStock()
        {
            List<PickList> pickLists = _service.AllocateInventory(new List<OrderLine>
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
            _service.AllocateInventory(new List<OrderLine>
            {
                new OrderLine
                {
                    Customer = _clinic,
                    Product = _procrit,
                    Quantity = 3
                }
            });

            Assert.AreEqual(4, _warehouse1.GetInventoryOnHand(_procrit));
        }
    }
}
