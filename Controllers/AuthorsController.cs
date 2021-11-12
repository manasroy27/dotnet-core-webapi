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
    public class AuthorsController : Controller
    {
        private readonly BookContext _context;

        public AuthorsController(BookContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Author>> GetAuthors()
        {
            return _context.Authors.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // POST: api/Authours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor(AuthorDTO AuthorDTO)
        {
            var author = new Author
            {
                Id = AuthorDTO.Id,
                Name = AuthorDTO.Name
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAuthor),
                new { id = author.Id },
                author);
        }

        // PUT: api/Authors/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int isbn, BookDTO BookDTO)
        {
            if (isbn != BookDTO.ISBN)
            {
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(isbn);
            if (book == null)
            {
                return NotFound();
            }

            book.Price = BookDTO.Price;

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

        // DELETE: api/Authors/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(long id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
