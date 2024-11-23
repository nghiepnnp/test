using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User : BaseEntities
    {
        [Required]
        [MaxLength(50)]
        public string? UserName { get; set; }

        [Required]
        [MaxLength(256)]
        public string? Password { get; set; }

        public string? Role { get; set; } = "User";

    }
}
