using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    public class AllTimeTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string  BookTitle { get; set; }

        [Required]
        public required string MemberName { get; set; }

        [Required]
        public DateTime IssuedDate { get; set; }

        public DateTime? ReturnedDate { get; set; }  // Nullable
    }
}
