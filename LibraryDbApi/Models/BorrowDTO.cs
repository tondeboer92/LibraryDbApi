namespace LibraryDbApi.Models
{
    public class BorrowDTO
    {
        public int BorrowId { get; set; }
        public int BookId { get; set; }
        public int BorrowerId { get; set; }
        public DateTime LoanDate { get; set; }
    }
}
