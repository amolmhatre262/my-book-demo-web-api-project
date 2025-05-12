using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_Books.Data;
using My_Books.Data.Models;

namespace My_Books.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        #region Code get data from table

        private readonly AppDbContext _context;

        public WeatherForecastController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _context.Book.ToListAsync(); // EF Core sends SELECT * FROM Book
        }

        #endregion

        #region Code Insert data into table

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            book.DateAdded = DateTime.Now;

            await _context.Book.AddAsync(book);
            await _context.SaveChangesAsync();

            return Ok(book); // or return CreatedAtAction(...) if you want to return 201
        }

        #endregion

        #region Code Update data in table

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookById(int id, [FromBody] Book updatedBook)
        {
            if (id != updatedBook.ID)
                return BadRequest("Book ID mismatch");

            var book = await _context.Book.FindAsync(id);
            if (book == null)
                return NotFound("Book not found");

            // Update properties
            book.Title = updatedBook.Title;
            book.Description = updatedBook.Description;
            book.IsRead = updatedBook.IsRead;
            book.DateRead = updatedBook.DateRead;
            book.Rate = updatedBook.Rate;
            book.Gener = updatedBook.Gener;
            book.Author = updatedBook.Author;
            book.CoverURL = updatedBook.CoverURL;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error updating the book.");
            }

            return NoContent();
        }
        #endregion

        #region Code Delete data from table
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookById(int id)
        {
            var book = await _context.Book.FindAsync(id);

            if (book == null)
                return NotFound("Book not found");

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return Ok("Book deleted successfully");
        }

        #endregion

        

    }
}
