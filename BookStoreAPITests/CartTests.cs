using BookStoreAPI.Controllers;
using BookStoreAPI.Data.Repositories;
using BookStoreAPI.Models;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Security.Claims;

namespace BookStoreAPITests
{
    public class CartTests
    {
        private readonly Mock<ILogger<CartController>> logger = new Mock<ILogger<CartController>>();

        [Fact]
        public async Task GetCart_ShouldBe_Ok()
        {
            // mock user
            var mockContext = new Mock<HttpContext>(MockBehavior.Strict);
            mockContext.SetupGet(hc => hc.User).Returns(
                new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, "mock user id"),
                new Claim(ClaimTypes.Name, "mock user name")
            })));

            var cartController = new CartController(new StubShoppingCartRepository(), new StubBookRepository(), logger.Object);

            cartController.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };

            var okResult = (OkObjectResult)await cartController.GetCart();

            Assert.True(((int)HttpStatusCode.OK).CompareTo(okResult.StatusCode) == 0);
            Assert.NotNull(okResult.Value);
            var items = (List<ShoppingCartItem>)okResult.Value;
            Assert.NotNull(items);
            Assert.True((items.Count.CompareTo(2) == 0));
        }

        [Fact]
        public async Task GetCart_NoUser_ShouldBe_Unauthorized()
        {
            // mock user
            var mockContext = new Mock<HttpContext>(MockBehavior.Strict);
            mockContext.SetupGet(hc => hc.User).Returns(
                new ClaimsPrincipal());

            var cartController = new CartController(new StubShoppingCartRepository(), new StubBookRepository(), logger.Object);

            cartController.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };

            var unauthorizedResult = (UnauthorizedResult)await cartController.GetCart();

            Assert.True(((int)HttpStatusCode.Unauthorized).CompareTo(unauthorizedResult.StatusCode) == 0);
        }

        [Fact]
        public async Task AddBookToCart_ShouldBe_Ok()
        {
            // mock user
            var mockContext = new Mock<HttpContext>(MockBehavior.Strict);
            mockContext.SetupGet(hc => hc.User).Returns(
                new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, "mock user id"),
                new Claim(ClaimTypes.Name, "mock user name")
            })));

            var cartController = new CartController(new StubShoppingCartRepository(), new StubBookRepository(), logger.Object);

            cartController.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };

            var okResult = (OkResult)await cartController.AddBookToCart(1);

            Assert.True(((int)HttpStatusCode.OK).CompareTo(okResult.StatusCode) == 0);
        }

        [Fact]
        public async Task AddBookToCart_NoUser_ShouldBe_Unauthorized()
        {
            // mock user
            var mockContext = new Mock<HttpContext>(MockBehavior.Strict);
            mockContext.SetupGet(hc => hc.User).Returns(
                new ClaimsPrincipal());

            var cartController = new CartController(new StubShoppingCartRepository(), new StubBookRepository(), logger.Object);

            cartController.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };

            var unauthorizedResult = (UnauthorizedResult)await cartController.AddBookToCart(1);

            Assert.True(((int)HttpStatusCode.Unauthorized).CompareTo(unauthorizedResult.StatusCode) == 0);
        }


        [Fact]
        public async Task AddBookToCart_NoBook_ShouldBe_NotFound()
        {
            // mock user
            var mockContext = new Mock<HttpContext>(MockBehavior.Strict);
            mockContext.SetupGet(hc => hc.User).Returns(
                new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, "mock user id"),
                new Claim(ClaimTypes.Name, "mock user name")
            })));

            var mockBookRepository = new Mock<IBookRepository>();
            mockBookRepository.Setup(mr => mr.GetBookByIdAsync(It.IsAny<int>())).Returns(() => Task.FromResult<Book>(null));

            var cartController = new CartController(new StubShoppingCartRepository(), mockBookRepository.Object, logger.Object);

            cartController.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };

            var notFoundResult = (NotFoundResult)await cartController.AddBookToCart(1);

            Assert.True(((int)HttpStatusCode.NotFound).CompareTo(notFoundResult.StatusCode) == 0);
        }
    }
}
