using BookStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Data.Repositories
{
    public class StubBookRepository : IBookRepository
    {
        public DbSet<Book> dbSet => default;

        public async Task AddBookAsync(Book book)
        {
            book.Id = 1;
            await Task.CompletedTask;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await Task.FromResult(new List<Book>
            {
                new Book { Id = 1, Title = "Mock Book1", Author = "Mocker1", Category = "Mock Category1", Price = new decimal(9.99) },
                new Book { Id = 2, Title = "Mock Book2", Author = "Mocker2", Category = "Mock Category2", Price = new decimal(19.99) }
            });
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await Task.FromResult(new Book { Id = id, Title = "Mock Book", Author = "Mocker", Category = "Mock Category", Price = new decimal(9.99) });
        }
    }
}
