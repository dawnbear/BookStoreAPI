using BookStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Data.Repositories
{
    public interface IBookRepository
    {
        DbSet<Book> dbSet { get; }

        Task AddBookAsync(Book book);

        Task<List<Book>> GetAllBooksAsync();

        Task<Book> GetBookByIdAsync(int id);
    }
}
