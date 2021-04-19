using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Threading.Tasks;

namespace ECommerce.ProductCatalog.Model
{
    public interface IProductCatalogService : IService
    {
        Task AddProduct(Product product);
        Task<Product> GetProduct(Guid productId);
        Task<Product[]> GetAllProducts();
    }
}
