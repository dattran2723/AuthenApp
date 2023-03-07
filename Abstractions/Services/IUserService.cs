using Dtos;
using Dtos.InputDtos;

namespace Abstractions.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(int id);

        List<UserDto> GetAll();

        Task<UserDto> CreateUserAsync(CreateUserDto user);

        Task<bool> IsExistEmailAsync(string email);

        Task<UserDto?> AuthenticateAsync(LoginDto dto);
    }
}
