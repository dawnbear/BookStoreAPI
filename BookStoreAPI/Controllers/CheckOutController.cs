using BookStoreAPI.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStoreAPI.Controllers
{
    [Route("api/[Controller]")]
    public class CheckOutController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public CheckOutController(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CalculateTotalPrice()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var cart = await _shoppingCartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                return NotFound();
            }

            decimal totalPrice = cart.Items.Sum(o => o.Book.Price * o.Quantity);

            return Ok(totalPrice);
        }
    }
}
