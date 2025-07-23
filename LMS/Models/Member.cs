namespace LMS.Models
{
    public class Member
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public bool IsBlocked { get; set; } = false;

    }

}
