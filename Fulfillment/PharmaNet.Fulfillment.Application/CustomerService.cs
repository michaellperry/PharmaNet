using System.Linq;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Infrastructure.Repository;

namespace PharmaNet.Fulfillment.Application
{
    public class CustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Customer GetCustomer(string customerName, string customerAddress)
        {
            Customer customer = _customerRepository.GetAll()
                .FirstOrDefault(c => c.Name == customerName);
            if (customer == null)
            {
                customer = _customerRepository.Add(new Customer
                {
                    Name = customerName,
                    ShippingAddress = customerAddress
                });
                _customerRepository.SaveChanges();
            }
            return customer;
        }
    }
}
