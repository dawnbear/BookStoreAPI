# Bookstore API

## Introduction
* This is a REST API for an online bookstore developed using.NET 8. 
* The API focuses on essential functionalities such as managing books, handling shopping cart operations.
* The API implementing security features like authentication and authorization. 
* The API also provides cloud-native support through containerization with Docker and implements OpenTelemetry for telemetry data collection.

## Setup and Run Instructions
1. Clone the repository from GitHub.
2. Ensure you have.NET 8 SDK installed on your machine.
3. Build the solution using your preferred IDE or the command line: dotnet build.
4. To run the application in development mode, use: dotnet run.
5. For production deployment, you can use Docker. 
    * Build the Docker image using the provided Dockerfile: docker build -t bookstoreimage ..\..\BookStoreAPI\BookStoreAPI 
    * Then run the container: docker run -p 80:8080 bookstoreimage.
    * Start access the API by http://localhost/swagger/index.html .
 
## API Usage Guide

**At first. You should register an account and login to get token for access other APIS.**

1. Account API
    * Register: Send a POST request to /api/account/register with the following attributes in the request body:
        * userName: the account name
        * password: the password for the account
        * example:
            {
                "userName": "admin",
                "password": "Pwd123."
            } 
    * Login:  Send a POST request to /api/account/login with the following attributes in the request body:
        * userName: the account name
        * password: the password for the account
        * example:
            {
                "userName": "admin",
                "password": "Pwd123."
            } 
        * token: return the response body if login successful.

**Pleaes add "Bearer " in front of the token(note that there is a space after the Bearer) for Authorization for following access**

**If you use swagger page. The Authorize button can be found in the top right corner of the page. Click Open and fill in the spliced token**

**If you use PostMan. Add a new item in the request header as key:Authorization value:the spliced token**

2. Book API
    * Add a book: Send a POST request to /api/book with the following attributes in the request body:
        * title: The title of the book.
        * author: The author of the book.
        * category: The category of the book.
        * price: The price of the book.
        * example:
            {
            "title": "Journey",
            "author": "Wu Chenen",
            "category": "novel",
            "price": 99.99
            }
    *  Retrieve all books: Send a GET request to /api/book to get a list of all books.

3. Shopping Cart API
    * Add a book to cart: Send a POST request to /api/cart/{bookId} with the book ID in the request body.
        * bookId can get from Book API.
        * example: /api/cart/1
    * Retrieve cart contents: Send a GET request to /api/cart to get the contents of the shopping cart.

4. Checkout API
    * Send a GET request to /api/checkout to calculate the total price of items in the shopping cart.
    
## Testing Instructions
* Unit and integration tests have been written using xUnit for the.NET platform.
* To run the tests, use your preferred IDE or the command line: dotnet test.

##
* This README provides all the necessary information for setting up, running the API. 
* For further inquiries or issues, please refer to the project's GitHub repository.