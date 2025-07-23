using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;

        [Required]
        [Range(1000, 9999, ErrorMessage = "Enter a valid year")]
        [CustomValidation(typeof(Book), nameof(ValidateYearNotFuture))]
        public int Year { get; set; }
        public static ValidationResult ValidateYearNotFuture(int year, ValidationContext context)
        {
            if (year > DateTime.Now.Year)
            {
                return new ValidationResult("Year cannot be greater than current year.");
            }

            return ValidationResult.Success;
        }
        public string? Location { get; set; }
        public string Status { get; set; } = "Available";    
    }
}