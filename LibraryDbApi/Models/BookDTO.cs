namespace LibraryDbApi.Models
{
    public class BookDTO
    {
        public int BookId { get; set; }
        public string? Title { get; set; }
        public string? ISBN { get; set; }
        public int PublicationYear { get; set; }
        public int Rating { get; set; }
        public int Copies { get; set; }
    }
}