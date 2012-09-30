using System.Linq;
using PharmaNet.Fulfillment.Domain;
using PharmaNet.Infrastructure.Repository;

namespace PharmaNet.Fulfillment.Application
{
    public class ProductService
    {
        private IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public Product GetProduct(int productNumber)
        {
            return _productRepository.GetAll()
                .FirstOrDefault(p => p.ProductNumber == productNumber);
        }
    }
}
