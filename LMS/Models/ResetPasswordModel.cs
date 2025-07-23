using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}
