using BookStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookstoreDbContext _dbContext;

        public DbSet<Book> dbSet => _dbContext.Books;

        public BookRepository(BookstoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddBookAsync(Book book)
        {
            await _dbContext.Books.AddAsync(book);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }
    }
}
