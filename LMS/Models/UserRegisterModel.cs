using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class UserRegisterModel
    {
        [Required]
        public required string Name { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required, MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public required string Password { get; set; }
    }
}
