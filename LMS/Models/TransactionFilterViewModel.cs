namespace LMS.Models
{
    public class TransactionFilterViewModel
    {
        public string Filter { get; set; } = "Not Returned";
        public List<AllTimeTransaction> Transactions { get; set; } = new();

        //public int TotalIssued { get; set; }
        //public int TotalReturned { get; set; }
        //public int TotalNotReturned { get; set; }
    }
}
