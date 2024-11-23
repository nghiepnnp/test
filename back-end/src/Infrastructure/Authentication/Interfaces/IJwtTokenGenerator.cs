using Domain.DTOs;

namespace Infrastructure.Authentication.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(UserDto user);
    }
}
