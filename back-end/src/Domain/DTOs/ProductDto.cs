using Domain.Entities;
using Microsoft.AspNetCore.Http;


namespace Domain.DTOs
{
    public class ProductDto : Product
    {
        public IEnumerable<IFormFile>? RawFiles { get; set; } = null;
    }
}
