using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AuthentifyCore.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
