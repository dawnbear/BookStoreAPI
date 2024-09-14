using BookStoreAPI.Controllers;
using BookStoreAPI.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Security.Claims;

namespace BookStoreAPITests
{
    public class CheckOutTests
    {
        [Fact]
        public async Task CalculateTotalPrice_ShouldBe_Ok()
        {
            // mock user
            var mockContext = new Mock<HttpContext>(MockBehavior.Strict);
            mockContext.SetupGet(hc => hc.User).Returns(
                new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, "mock user id"),
                new Claim(ClaimTypes.Name, "mock user name")
            })));

            var checkOutController = new CheckOutController(new StubShoppingCartRepository());

            checkOutController.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };

            var okResult = (OkObjectResult)await checkOutController.CalculateTotalPrice();

            Assert.True(((int)HttpStatusCode.OK).CompareTo(okResult.StatusCode) == 0);
            Assert.NotNull(okResult.Value);
            var totalPrice = (decimal)okResult.Value;
            Assert.True((totalPrice.CompareTo(new decimal(29.98)) == 0));
        }

        [Fact]
        public async Task CalculateTotalPrice_NoUser_ShouldBe_Unauthorized()
        {
            // mock user
            var mockContext = new Mock<HttpContext>(MockBehavior.Strict);
            mockContext.SetupGet(hc => hc.User).Returns(
                new ClaimsPrincipal());

            var checkOutController = new CheckOutController(new StubShoppingCartRepository());

            checkOutController.ControllerContext = new ControllerContext()
            {
                HttpContext = mockContext.Object
            };

            var unauthorizedResult = (UnauthorizedResult)await checkOutController.CalculateTotalPrice();

            Assert.True(((int)HttpStatusCode.Unauthorized).CompareTo(unauthorizedResult.StatusCode) == 0);
        }
    }
}
