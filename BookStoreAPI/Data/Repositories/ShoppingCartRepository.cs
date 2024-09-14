using BookStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Data.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly BookstoreDbContext _dbContext;

        public DbSet<ShoppingCart> cartDbSet => _dbContext.ShoppingCarts;

        public DbSet<ShoppingCartItem> cartItemDbSet => _dbContext.ShoppingCartItems;
        public ShoppingCartRepository(BookstoreDbContext dbContext)
        { 
            _dbContext = dbContext; 
        }

        public async Task AddCart(ShoppingCart cart)
        {
            await cartDbSet.AddAsync(cart);

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddCartItem(ShoppingCartItem cartItem)
        {
            await cartItemDbSet.AddAsync(cartItem);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ShoppingCart> GetByUserIdAsync(string userId)
        {
            return await cartDbSet.FirstOrDefaultAsync(cart => cart.UserId.Equals(userId));
        }

        public async Task IncreaseBookQuantity(ShoppingCartItem cartItem)
        {
            cartItem.Quantity++;
            await _dbContext.SaveChangesAsync();
        }
    }
}
