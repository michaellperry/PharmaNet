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
        private IRepository<Product> _productRepository;

        public InventoryAllocationService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public List<PickList> AllocateInventory(List<OrderLine> orderLines)
        {
            return new List<PickList>();
        }
    }
}
