using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Comment : BaseEntities
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public string? Content { get; set; } 

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string? UserName { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
