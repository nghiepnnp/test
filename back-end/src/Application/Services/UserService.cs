using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Authentication.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<UserService> _logger;
        private readonly IJwtTokenGenerator _jwtService;

        public UserService(AppDbContext dbContext, ILogger<UserService> logger, IJwtTokenGenerator jwtService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _jwtService = jwtService;
        }

        public async Task<string> Login(UserDto userDto)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userDto.UserName);
                if (user == null || !BCrypt.Net.BCrypt.Verify(userDto?.Password, user?.Password))
                {
                    throw new Exception("Invalid username or password");
                }

                userDto!.Id = user!.Id;
                userDto.Role = user.Role;

                return _jwtService.GenerateToken(userDto!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task Register(UserDto userDto)
        {
            try
            {
                if (await _dbContext.Users.AnyAsync(u => u.UserName == userDto.UserName))
                    throw new Exception("Username already exists");

                // Hash pass
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

                var user = new User
                {
                    UserName = userDto.UserName,
                    Password = hashedPassword
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
