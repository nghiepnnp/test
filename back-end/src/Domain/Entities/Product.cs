using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Product : BaseEntities
    {
        [Required]
        [MaxLength(256)]
        public string? Title { get; set; }

        [MaxLength(1024)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? Images { get; set; }

        public ICollection<Comment>? Comments { get; set; }

    }
}
