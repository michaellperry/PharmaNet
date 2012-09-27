using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PharmaNet.Fulfillment.Application;
using PharmaNet.Fulfillment.Domain;

namespace PharmaNet.Fulfillment.Test
{
    [TestClass]
    public class InventoryAllcationServiceTest
    {
        private InventoryAllocationService _service;

        [TestInitialize]
        public void Initialize()
        {
            _service = new InventoryAllocationService();
        }

        [TestMethod]
        public void CanAllocateFromOneWarehouse()
        {
            Product procrit = new Product { ProductId = 11190 };
            Customer clinic = new Customer
            {
                Name = "The Clinic",
                ShippingAddress = "1 My Way"
            };
            List<PickList> pickLists = _service.AllocateInventory(new List<OrderLine>
            {
                new OrderLine
                {
                    Customer = clinic,
                    Product = procrit,
                    Quantity = 3
                }
            });

            Assert.AreEqual(1, pickLists.Count);
            Assert.AreEqual(11190, pickLists[0].Product.ProductId);
            Assert.AreEqual(3, pickLists[0].Quantity);
        }
    }
}
