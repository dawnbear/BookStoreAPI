using BookStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Data.Repositories
{
    public class StubShoppingCartRepository : IShoppingCartRepository
    {
        public DbSet<ShoppingCart> cartDbSet => default;

        public DbSet<ShoppingCartItem> cartItemDbSet => default;

        public async Task AddCart(ShoppingCart cart)
        {
            await Task.CompletedTask;
        }

        public async Task AddCartItem(ShoppingCartItem cartItem)
        {
            await Task.CompletedTask;
        }

        public async Task<ShoppingCart> GetByUserIdAsync(string userId)
        {
            return await Task.FromResult(new ShoppingCart
            {
                Id = 1,
                UserId = userId,
                Items = new List<ShoppingCartItem>
                {
                    new ShoppingCartItem
                    {
                        Id = 1,
                        BookId = 1,
                        Book = new Book { Id = 1, Title = "Mock Book1", Author = "Mocker1", Category = "Mock Category1", Price = new decimal(9.99) },
                        Quantity = 1
                    },
                    new ShoppingCartItem
                    {
                        Id = 2,
                        BookId = 2,
                        Book = new Book { Id = 2, Title = "Mock Book2", Author = "Mocker2", Category = "Mock Category2", Price = new decimal(19.99) },
                        Quantity = 1
                    },
                }
            });
        }

        public async Task IncreaseBookQuantity(ShoppingCartItem cartItem)
        {
            cartItem.Quantity++;

            await Task.CompletedTask;
        }
    }
}
