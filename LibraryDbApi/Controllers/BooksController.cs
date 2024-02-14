using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryDbApi.Models;

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

        // GET: api/books
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _context.Books.ToList();
            return Ok(books);
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            var book = _context.Books.Find(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // POST: api/books
        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Invalid book data.");
            }

            _context.Books.Add(book);
            _context.SaveChanges();

            return Ok($"Book '{book.Title}' has been added to the library.");
        }


    }
}
