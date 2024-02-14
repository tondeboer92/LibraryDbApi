﻿using System.Text.Json.Serialization;

namespace LibraryDbApi.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string? Title { get; set; }
        public int ISBN { get; set; }
        public int PublicationYear { get; set; }
        public int Rating { get; set; }
        
        
                
        public List<Author> Authors { get; set; }


    }

}
