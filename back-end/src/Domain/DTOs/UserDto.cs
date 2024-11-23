using Domain.Entities;

namespace Domain.DTOs
{
    public class UserDto : User
    {
    }

    public class LoginResponse
    {
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
