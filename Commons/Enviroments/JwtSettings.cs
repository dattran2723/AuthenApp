using Microsoft.IdentityModel.Tokens;

namespace Commons.Enviroments
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public int ExpirationInHours { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime Expiration => IssuedAt.AddHours(ExpirationInHours);
        public int RefreshTokenExpirationInDays { get; set; }
        public DateTime RefreshTokenExpiration => IssuedAt.AddDays(RefreshTokenExpirationInDays);
        public SigningCredentials? SigningCredentials { get; set;}
    }
}
