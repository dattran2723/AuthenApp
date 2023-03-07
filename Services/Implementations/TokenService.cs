using Abstractions.Services;
using Abstractions.Sessions;
using Abstractions.UnitOfWork;
using Commons.Enviroments;
using Dtos;
using Entities.Entities;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Services.Implementations
{
    public class TokenService : ITokenService
    {
        #region Vairables

        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;
        private readonly IUserService _userService;
        private readonly IAuthSession _authSession;
        private IRepository<Token> _tokenRepository => _unitOfWork.GetRepository<Token>();

        #endregion

        #region Constructor
        public TokenService(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtSettings, 
                            IUserService userService, IAuthSession authSession)
        {
            _jwtSettings = jwtSettings.Value;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _authSession = authSession;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Create new token and refresh token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<LoginResponse> RequestTokenAsync(UserDto user)
        {
            _jwtSettings.IssuedAt = DateTime.Now;

            var response = new LoginResponse
            {
                User = user,
                Token = GenerateToken(user),
                RefreshToken = await GenerateRefreshToken(user.Id),
            };

            _unitOfWork.Complete();

            return response;
        }

        /// <summary>
        /// Get token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<Token?> GetByRefreshTokenAsync(string refreshToken)
        {
            var token = await _tokenRepository.GetFirstOrDefaultAsync(x => x.RefreshToken.Equals(refreshToken));

            return token;
        }

        /// <summary>
        /// Regenerate token and refreshToken
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<RefreshTokenResponse> RefreshTokenAsync(Token token)
        {
            var user = await _userService.GetByIdAsync(token.UserId);

            await InvalidateOldRefreshToken(token);

            var loginInfo = await RequestTokenAsync(user);

            var result = new RefreshTokenResponse
            {
                Token = loginInfo.Token,
                RefreshToken = loginInfo.RefreshToken,
            };

            _unitOfWork.Complete();

            return result;
        }

        /// <summary>
        /// Remove refreshToken of user
        /// </summary>
        /// <returns></returns>
        public async Task RemoveRefreshTokenAsync()
        {
            var userId = _authSession.GetUserId();

            await _tokenRepository.DeleteAsync(x => x.UserId == userId);

            _unitOfWork.Complete();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Generate token and write token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GenerateToken(UserDto user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString())
            };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                userClaims,
                _jwtSettings.IssuedAt,
                _jwtSettings.Expiration,
                _jwtSettings.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// Generate refresh token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<string> GenerateRefreshToken(int userId)
        {
            var token = new Token
            {
                UserId = userId,
                RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresIn = _jwtSettings.RefreshTokenExpiration.ToString(),
                CreatedAt = DateTime.Now
            };

            await _tokenRepository.InsertAsync(token);

            return token.RefreshToken;
        }

        /// <summary>
        /// Invalidate the old refreshToken
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task InvalidateOldRefreshToken(Token token)
        {
            token.ExpiresIn = DateTime.Now.ToString();
            token.UpdatedAt = DateTime.Now;

            await _tokenRepository.UpdateAsync(token);
        }
        #endregion
    }
}
