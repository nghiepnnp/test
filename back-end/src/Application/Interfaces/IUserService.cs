using Domain.DTOs;

namespace Application.Interfaces
{
    public interface IUserService
    {
        public Task<string> Login(UserDto user);
        public Task Register(UserDto user);
    }
}
