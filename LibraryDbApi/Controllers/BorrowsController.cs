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
        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowDTO>> GetBorrow(int id)
        {
            var borrow = await _context.Borrows.FindAsync(id);

            if (borrow == null)
            {
                return NotFound();
            }

            var borrowDto = new BorrowDTO
            {
                BookId = borrow.BookId,
                BorrowerId = borrow.BorrowerId,
                LoanDate = borrow.LoanDate,
                ReturnDate = borrow.ReturnDate
            };

            return CreatedAtAction(nameof(GetBorrow), new { id = borrow.BorrowId }, borrowDto);

        }

        // PUT: api/Borrows/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBorrow(int id, BorrowDTO borrowDto)
        {
            var borrow = await _context.Borrows.FindAsync(id);

            if (borrow == null)
            {
                return NotFound();
            }

            borrow.LoanDate = borrowDto.LoanDate;
            borrow.ReturnDate = borrowDto.ReturnDate;

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
        [HttpPost]
        public async Task<ActionResult<BorrowDTO>> PostBorrow(BorrowDTO borrowDto)
        {

            var book = await _context.Books.FindAsync(borrowDto.BookId);

            if (book == null)
            {
                return NotFound("Book ia null");
            }
            if (book.Copies <= 0)
            {
                return BadRequest("No copies available");
            }


            // Decrement the book's available copies
            book.Copies--;

            // Add the borrow record to the DbContext
            var borrow = new Borrow
            {
                BookId = borrowDto.BookId,
                BorrowerId = borrowDto.BorrowerId,
                LoanDate = borrowDto.LoanDate,
            };

            // Add the new borrow record to the DbContext
            _context.Borrows.Add(borrow);

            // Save all changes to the DbContext
            await _context.SaveChangesAsync();



            // Return a 201 Created response, with the location header set to the URI of the new borrow
            return CreatedAtAction(nameof(GetBorrow), new { id = borrow.BorrowId }, borrowDto);
        }


        // PUT: api/Borrows/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/return")]
        public async Task<ActionResult<BorrowDTO>> ReturnBorrow(int id)
        {
            var borrow = await _context.Borrows.FindAsync(id);
            if (borrow == null)
            {
                return BadRequest("Not found");
            }
            if (borrow.ReturnDate.HasValue)
            {
                return BadRequest("Error. Already returned!");
            }

            var book = await _context.Books.FindAsync(borrow.BookId);

            if (book == null)
            {
                return BadRequest("Book not found");
            }
            if (book == null)
            {
                return NotFound("Associated book not found.");
            }


            book.Copies++;
            borrow.ReturnDate = DateTime.Today;


            await _context.SaveChangesAsync();

            return Ok(new { message = "Book returned" });
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

