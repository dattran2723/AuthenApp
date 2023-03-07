using Dtos;
using Entities.Entities;

namespace Abstractions.Services
{
    public interface ITokenService
    {
        Task<LoginResponse> RequestTokenAsync(UserDto user);

        Task<Token?> GetByRefreshTokenAsync(string refreshToken);

        Task<RefreshTokenResponse> RefreshTokenAsync(Token token);

        Task RemoveRefreshTokenAsync();
    }
}
