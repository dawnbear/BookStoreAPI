using BookStoreAPI.Controllers;
using BookStoreAPI.Data.Repositories;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookStoreAPITests
{
    public class BookTests
    {
        [Fact]
        public async Task AddBook_ShouldBe_Ok()
        {
            var bookController = new BookController(new StubBookRepository());
            var book = new Book { Title = "Mock Book", Author = "Mocker", Category = "Mock Category", Price = new decimal(9.99) };
            var result = await bookController.AddBook(book);

            var createdResult = (CreatedAtActionResult)result;

            Assert.NotNull(createdResult);
            Assert.Equal(nameof(bookController.AddBook), createdResult.ActionName);
            Assert.NotNull(createdResult.RouteValues);
            Assert.Equal(0, book.Id.CompareTo(createdResult.RouteValues["id"]));
            Assert.Equal(book, createdResult.Value);
        }

        [Fact]
        public async Task GetBook_ShouldBe_Ok()
        {
            var bookController = new BookController(new StubBookRepository());
            var okResult = (OkObjectResult)await bookController.GetBooks();

            Assert.NotNull(okResult);
            Assert.True(((int)HttpStatusCode.OK).CompareTo(okResult.StatusCode) == 0);
            Assert.NotNull(okResult.Value);
            var books = (List<Book>)okResult.Value;
            Assert.True((books.Count.CompareTo(2) == 0));
        }
    }
}
