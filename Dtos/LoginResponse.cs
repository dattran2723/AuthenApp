namespace Dtos
{
    public class LoginResponse
    {
        public UserDto? User { get; set; }

        public string? Token { get; set; }

        public string? RefreshToken { get; set; }
    }
}
