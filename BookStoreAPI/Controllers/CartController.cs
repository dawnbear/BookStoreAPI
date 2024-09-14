using BookStoreAPI.Data.Repositories;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace BookStoreAPI.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    public class CartController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<CartController> _logger;

        public CartController(IShoppingCartRepository shoppingCartRepository, IBookRepository bookRepository, ILogger<CartController> logger)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _logger.LogInformation("api/Cart GetCart Unauthorized user.");
                return Unauthorized();
            }

            var cart = await _shoppingCartRepository.GetByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogInformation($"api/Cart GetCart no cart for user {userId}.");
                return NotFound();
            }

            return Ok(cart.Items);
        }

        [HttpPost("{bookId}")]
        public async Task<IActionResult> AddBookToCart(int bookId)
        {
            using var activitySource = new ActivitySource("BookSotreAPI");
            using var activity = activitySource.StartActivity("cart");
            var meter = new Meter("BookStoreAPI");
            var counter = meter.CreateCounter<long>("counter");

            try
            {
                activity?.SetTag("check book", 1);
                // find book
                var book = await _bookRepository.GetBookByIdAsync(bookId);
                if (book == null)
                {
                    counter.Add(100);
                    _logger.LogInformation($"api/Cart AddBookToCart Book not found {bookId}.");
                    return NotFound();
                }

                activity?.SetTag("check user", 2);
                // find or create cart            
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    counter.Add(100);
                    _logger.LogInformation("api/Cart AddBookToCart Unauthorized user.");
                    return Unauthorized();
                }

                activity?.SetTag("check cart", 3);
                var cart = await _shoppingCartRepository.GetByUserIdAsync(userId);

                if (cart == null)
                {
                    counter.Add(100);
                    activity?.SetTag("create new cart", 4);
                    cart = new ShoppingCart() { UserId = userId, Items = new List<ShoppingCartItem>() };
                    await _shoppingCartRepository.AddCart(cart);

                    _logger.LogInformation($"api/Cart AddBookToCart new cart added for {userId}.");
                }

                activity?.SetTag("check cart item", 5);
                var cartItem = cart.Items.FirstOrDefault(o => o.BookId == bookId);

                if (cartItem == null)
                {
                    counter.Add(100);
                    activity?.SetTag("create new cart item", 6);
                    cartItem = new ShoppingCartItem() { BookId = bookId, Book = book, Quantity = 1 };
                    cart.Items.Add(cartItem);

                    await _shoppingCartRepository.AddCartItem(cartItem);
                }
                else
                {
                    counter.Add(100);
                    activity?.SetTag("increase cart item quantity", 7);
                    await _shoppingCartRepository.IncreaseBookQuantity(cartItem);
                }

                _logger.LogInformation($"api/Cart AddBookToCart Book {book.Title} added to cart with quantity {cartItem.Quantity}.");

                return Ok();
            }
            catch (Exception ex)
            {
                counter.Add(100);
                activity?.SetTag("exception", 9);
                _logger.LogError(ex, "An error occurred while add book to cart.");
                return BadRequest(ex);
            }
            finally
            {
                activity?.SetTag("end", 9);
                meter.Dispose();
                activity?.Dispose();
            }
        }
    }
}
