using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetAllProductsAsync();
        
        public Task<Product?> GetProductByIdAsync(Guid id);

        public Task AddProductAsync(ProductDto product);

        public Task UpdateProductAsync(ProductDto product);

        public Task DeleteProductAsync(Guid id);
    }
}
