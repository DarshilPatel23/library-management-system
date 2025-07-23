using System;

namespace LMS.Models
{
    public class ReturnedBook
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string MemberName { get; set; } = string.Empty; // Non-nullable reference type, initialized to empty string
        public DateTime IssuedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }  // Nullable


        // Navigation Property
        public Book Book { get; set; } = null!;
    }
}
