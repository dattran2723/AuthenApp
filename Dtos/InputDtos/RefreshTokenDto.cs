using System.ComponentModel.DataAnnotations;

namespace Dtos.InputDtos
{
    public class RefreshTokenDto
    {
        [Required(ErrorMessage = "RefreshToken is required")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
