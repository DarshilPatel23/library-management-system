namespace LMS.ViewModels
{
    public class TransactionViewModel
    {
        public required string BookTitle { get; set; }
        public required string MemberName { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public bool IsReturned => ReturnedDate.HasValue;
    }
}
