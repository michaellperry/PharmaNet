using System;
using System.Collections.Generic;
using System.Linq;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Infrastructure.Repository;

namespace PharmaNet.Fulfillment.Application
{
    public class PickListService
    {
        private readonly IRepository<PickList> _pickListRepository;

        public PickListService(IRepository<PickList> pickListRepository)
        {
            _pickListRepository = pickListRepository;
        }

        public void SavePickLists(List<PickList> pickLists)
        {
            foreach (var pickList in pickLists)
            {
                _pickListRepository.Add(pickList);
            }
            _pickListRepository.SaveChanges();
        }

        public List<PickList> GetPickLists(Guid orderId)
        {
            return _pickListRepository.GetAll()
                .Where(pl => pl.OrderId == orderId)
                .ToList();
        }
    }
}
