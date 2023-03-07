using System.ComponentModel.DataAnnotations;

namespace Dtos.InputDtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8-20 characters")]
        public string Password { get; set; } = string.Empty;
    }
}
