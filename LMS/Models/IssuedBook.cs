using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    public class IssuedBook
    {
        public int Id { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }

        public Book Book { get; set; } = null!; // Non-nullable reference type, initialized to null

        public string MemberName { get; set; } = string.Empty; // Non-nullable reference type, initialized to empty string

        public DateTime IssuedDate { get; set; }

    }
}
