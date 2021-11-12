using BooksApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly BookContext _context;

        public BooksController(BookContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all Books
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            return _context.Books?.ToList();
        }

        /// <summary>
        /// Get Book for a given ISBN
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        [HttpGet("{isbn}")]
        public async Task<ActionResult<Book>> GetBook(int isbn)
        {
            var book = await _context.Books.FindAsync(isbn);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        /// <summary>
        /// Create a new Book
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook(BookDTO bookDTO)
        {
            if(bookDTO.ISBN == 0)
            {
                return BadRequest("ISBN cannot be 0");
            }

            var author = _context.Authors.Find(bookDTO.AuthorId);
            if(author == null)
            {
                return BadRequest("Author does not exist");
            }
            var book = new Book
            {
                ISBN = bookDTO.ISBN,
                Title = bookDTO.Title,
                AuthorId = bookDTO.AuthorId,
                Author = author.Name,
                PublishedYear = bookDTO.PublishedYear,
                Price = bookDTO.Price,
                Currency = bookDTO.Currency
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBook),
                new { isbn = book.ISBN },
                book);
        }

        /// <summary>
        /// Update Book for specified ISBN
        /// </summary>
        /// <param name="isbn"></param>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int isbn, BookDTO bookDTO)
        {
            if (isbn != bookDTO.ISBN)
            {
                return BadRequest("ISBN does not match");
            }

            var book = await _context.Books.FindAsync(isbn);
            if (book == null)
            {
                return NotFound();
            }

            book.Price = bookDTO.Price;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!_context.Books.Any(e => e.ISBN == isbn))
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Delete book for given ISBN
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Books/1
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
    }
}
