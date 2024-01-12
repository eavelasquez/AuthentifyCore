using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AuthentifyCore.DTOs
{
    public class CreateUserDTO
    {
        [Required]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [PasswordPropertyText]
        public string Password { get; set; }
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
