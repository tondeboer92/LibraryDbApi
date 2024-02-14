using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryDbApi.Models;

namespace LibraryDbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BorrowsController(LibraryDbContext context)
        {
            _context = context;
        }

        //  when a GET request is made to `api/Borrows`, this method asynchronously retrieves all records from the `Borrows` table in the database and returns them as a list wrapped in an `ActionResult`. This allows clients of the API to receive all borrow records in a non-blocking manner.
        // GET: api/Borrows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Borrow>>> GetBorrows()
        {
            return await _context.Borrows.ToListAsync();
        }

        //This code snippet is part of a controller in an ASP.NET Core Web API application. It defines an action method that handles HTTP GET requests to retrieve a specific "Borrow" entity by its ID. Let's break down the key components
        // this action method provides a way for clients of the Web API to retrieve a specific borrow entity by its ID. If the entity exists, it is returned with an HTTP200 OK status; if not, an HTTP404 Not Found status is returned.     [HttpGet("{id}")]
        public async Task<ActionResult<Borrow>> GetBorrow(int id)
        {
            var borrow = await _context.Borrows.FindAsync(id);

            if (borrow == null)
            {
                return NotFound();
            }

            return borrow;
        }

        // PUT: api/Borrows/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBorrow(int id, Borrow borrow)
        {
            if (id != borrow.BorrowId)
            {
                return BadRequest();
            }

            _context.Entry(borrow).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowExists(id))
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

        // POST: api/Borrows
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Borrow>> PostBorrow(Borrow borrow)
        {
            var book = await _context.Books.FindAsync(borrow.BookId);
            if (book == null)
            {
                return BadRequest("Book not found");
            }
            if (book.CopiesAvailable <= 0)
            {
                return BadRequest("No copies available");
            }

            book.CopiesAvailable--;
            _context.Borrows.Add(borrow);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBorrow", new { id = borrow.BorrowId }, borrow);
        }


        // PUT: api/Borrows/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Borrow>> ReturnBorrow(int id, Borrow borrow)
        {

            if (id != borrow.BorrowId)
            {
                return BadRequest("Incorrect data");
            }

            var book = await _context.Books.FindAsync(borrow.BookId);
            

            if (book == null)
            {
                return BadRequest("Book not found");
            }

            book.CopiesAvailable++;
            borrow.ReturnDate = DateTime.Today;

            await _context.SaveChangesAsync();  

            return NoContent();
        }
    


        // DELETE: api/Borrows/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrow(int id)
        {
            var borrow = await _context.Borrows.FindAsync(id);
            if (borrow == null)
            {
                return NotFound();
            }

            _context.Borrows.Remove(borrow);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BorrowExists(int id)
        {
            return _context.Borrows.Any(e => e.BorrowId == id);
        }
    }
}
