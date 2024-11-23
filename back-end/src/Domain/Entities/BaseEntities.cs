using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class BaseEntities
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid(); 

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public Guid CreatedBy { get; set; }

        public Guid UpdateBy { get; set; }
    }
}
