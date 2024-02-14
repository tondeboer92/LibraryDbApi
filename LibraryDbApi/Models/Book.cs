namespace LibraryDbApi.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string? Title { get; set; }
        public string? ISBN { get; set; }
        public int PublicationYear { get; set; }
        public int Rating { get; set; }

        public int Copies { get; set; }
            

        public List<Borrow>? Borrows { get; set; }
        public List<Author>? Authors { get; set; }


    }

}
