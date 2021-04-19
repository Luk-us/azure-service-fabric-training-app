using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.ProductCatalog.Model
{
    public interface IProductRepository
    {
        Task AddProduct(Product product);
        Task<Product> GetProduct(Guid productId);
        Task<Product[]> GetAllProducts();
    }
}