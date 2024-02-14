using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryDbApi.Models
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Authors)
                .WithMany(a => a.Books)
                .UsingEntity(j => j.ToTable("BookAuthor"));
                

        }

        public DbSet<Borrower> Borrowers { get; set; } 
        public DbSet<Borrow> Borrows { get; set; }
    }
}
