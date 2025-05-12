using Microsoft.EntityFrameworkCore;
using My_Books.Data.Models;

namespace My_Books.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Book> Book { get; set; }

        #region Code to get data using Procedure
        public async Task<List<Book>> GetBooksUsingSP()
        {
            return await Book
                .FromSqlRaw("EXEC sp_Book_CRUD @Status = {0}", "G")
                .ToListAsync();
        }
        #endregion

    }
}
