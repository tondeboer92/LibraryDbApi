using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryDbApi.Models;
using System.Net;

namespace LibraryDbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }
            var bookDto = new BookDTO
            {
                BookId = book.BookId,
                Title = book.Title,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear,
                Rating = book.Rating,
                Copies = book.Copies
            };

            return bookDto;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookDTO bookDto)
        {
            
            if (id != bookDto.BookId)
            {
                return BadRequest("the ID don't match!");
            }

            var book = await _context.Books.FindAsync(id); 

            if (bookDto == null)
            {
                return BadRequest("the ID is null!");
            }

            

            book.Title = bookDto.Title;
            book.ISBN = bookDto.ISBN;
            book.PublicationYear = bookDto.PublicationYear;
            book.Rating = bookDto.Rating;
            book.Copies = bookDto.Copies;
            
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPut("{id}/AddCopy")]
        public async Task<IActionResult> AddCopy(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound("Book not found!");
            }

            book.Copies++; // Increment the Copies field

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound("Book not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // HTTP 204 No Content, indicating success
        }
        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookDTO>> PostBook(BookDTO bookDto)
        {
            var book = new Book
            {
                Title = bookDto.Title,
                ISBN = bookDto.ISBN,
                PublicationYear = bookDto.PublicationYear,
                Rating = bookDto.Rating,
                Copies = 1

            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.BookId }, new BookDTO
            {
                BookId = book.BookId,
                Title = book.Title,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear,
                Rating = book.Rating,
                Copies = book.Copies
            });
        }
        

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)  
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
