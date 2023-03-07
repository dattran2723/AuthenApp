using Abstractions.Sessions;
using Microsoft.AspNetCore.Http;
using System.Security.Authentication;
using System.Security.Claims;

namespace Commons.Sessions
{
    public class AuthSession : IAuthSession
    {
        #region Variables
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;
        #endregion

        #region Constuctor
        public AuthSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Get id of logged in user
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AuthenticationException"></exception>
        public int GetUserId()
        {
            var userIdClaim = Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
            if (string.IsNullOrEmpty(userIdClaim?.Value))
            {
                throw new AuthenticationException("User not login to system");
            }

            int userId;
            if (!int.TryParse(userIdClaim.Value, out userId))
            {
                throw new AuthenticationException("User not login to system");
            }

            return userId;
        }
        #endregion
    }
}
