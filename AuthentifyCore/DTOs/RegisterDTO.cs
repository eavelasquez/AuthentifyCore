using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AuthentifyCore.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [PasswordPropertyText]
        public string Password { get; set; }
        
        [PasswordPropertyText]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must be the same")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Name { get; set; }
        public string Lastname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public int Active { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
