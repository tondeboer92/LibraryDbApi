using System.Text.Json.Serialization;

namespace LibraryDbApi.Models
{
    public class Borrower
    {
        public int BorrowerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

    }
}
