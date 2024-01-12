using System.ComponentModel.DataAnnotations;

namespace AuthentifyCore.DTOs
{
    public class PatchUserDTO
    {        
        public string Username { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
