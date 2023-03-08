using Abstractions.Services;
using Dtos;
using Dtos.InputDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class UserController : ControllerBase
    {
        #region Variables
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        #endregion

        #region Constructor
        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Registration a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("sign-up")]
        [AllowAnonymous]
        [SwaggerResponse(((short)HttpStatusCode.Created), "", Type = typeof(UserDto))]
        public async Task<IActionResult> SignUp([FromBody] CreateUserDto user)
        {
            if(await _userService.IsExistEmailAsync(user.Email))
            {
                return BadRequest("Email already used");
            }

            var result = _userService.CreateUserAsync(user);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        /// <summary>
        /// User login
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("sign-in")]
        [AllowAnonymous]
        [SwaggerResponse(((short)HttpStatusCode.OK), "", Type = typeof(LoginResponse))]
        public async Task<IActionResult> SignIn([FromBody] LoginDto dto)
        {
            var user = await _userService.AuthenticateAsync(dto);

            if (user == null)
            {
                return BadRequest("Invalid username or password");
            }

            var response = _tokenService.RequestTokenAsync(user);

            return Ok(response);
        }

        /// <summary>
        /// Refresh token of user
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [SwaggerResponse(((short)HttpStatusCode.OK), "", Type = typeof(RefreshTokenResponse))]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            var token = await _tokenService.GetByRefreshTokenAsync(dto.RefreshToken);

            if (token == null)
            {
                return NotFound("Invalid Refresh Token.");
            }

            if(DateTime.Parse(token.ExpiresIn) < DateTime.Now)
            {
                return Unauthorized("Token Expired");
            }

            var response = await _tokenService.RefreshTokenAsync(token);

            return Ok(response);
        }

        /// <summary>
        /// User logout
        /// </summary>
        /// <returns></returns>
        [HttpPost("sign-out")]
        [SwaggerResponse(((short)HttpStatusCode.NoContent))]
        public async Task<IActionResult> Signout()
        {
            await _tokenService.RemoveRefreshTokenAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }
        #endregion
    }
}
