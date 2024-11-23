using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;
        private readonly IWebHostEnvironment _environment;

        public ProductService(AppDbContext dbContext, ILogger<ProductService> logger, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _logger = logger;
            _environment = environment;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                return await _dbContext.Products
                    .AsNoTracking()
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all products.");
                return Enumerable.Empty<Product>();
            }
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            try
            {
                return await _dbContext.Products
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Include(x => x.Comments!.OrderByDescending(y=>y.CreatedAt)) //.OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching product with ID: {id}");
                return null;
            }
        }

        public async Task AddProductAsync(ProductDto product)
        {
            try
            {
                product.Images = await UploadFile(product.RawFiles!);

                _dbContext.Products.Add(product);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new product.");
                throw;
            }
        }

        public async Task UpdateProductAsync(ProductDto product)
        {
            try
            {
                if (await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == product.Id) is Product outProduct)
                {
                    outProduct.Title = product.Title;
                    outProduct.Price = product.Price;
                    outProduct.Description = product.Description;
                    outProduct.UpdateBy = product.UpdateBy;
                    outProduct.UpdatedAt = DateTime.Now;

                    if (product.RawFiles != null && product.RawFiles.Any())
                    {
                        outProduct.Images = await UploadFile(product.RawFiles);
                    }

                    _dbContext.Products.Update(outProduct);
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating product with ID: {product.Id}");
                throw;
            }
        }

        public async Task DeleteProductAsync(Guid id)
        {
            try
            {
                var product = await _dbContext.Products.FindAsync(id);
                if (product != null)
                {
                    _dbContext.Products.Remove(product);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning($"Attempted to delete product with ID: {id}, but it was not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting product with ID: {id}");
                throw;
            }
        }

        public async Task<string> UploadFile(IEnumerable<IFormFile> files)
        {
            if (files != null && files.Any())
            {
                var uploadsFolder = Path.Combine(_environment.ContentRootPath, "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uploadedFilePaths = new List<string>();

                foreach (var rawImage in files)
                {
                    if (rawImage != null && rawImage.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}_{rawImage.FileName}";
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await rawImage.CopyToAsync(stream);
                        }

                        // Save path file upload
                        uploadedFilePaths.Add($"/Uploads/{fileName}");
                    }
                }

                return string.Join(":", uploadedFilePaths);
            }

            return string.Empty;
        }
    }
}
