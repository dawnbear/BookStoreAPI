using BookStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Data.Repositories
{
    public interface IShoppingCartRepository
    {
        DbSet<ShoppingCart> cartDbSet { get; }
        DbSet<ShoppingCartItem> cartItemDbSet { get; }

        Task AddCart(ShoppingCart cart);

        Task AddCartItem(ShoppingCartItem cartItem);

        Task IncreaseBookQuantity(ShoppingCartItem cartItem);

        Task<ShoppingCart> GetByUserIdAsync(string userId);
    }
}
