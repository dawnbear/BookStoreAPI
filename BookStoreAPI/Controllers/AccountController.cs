using BookStoreAPI.Models;
using BookStoreAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<BookstoreUser> _userManager;
        private readonly JwtTokenService _jwtTokenService;

        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<BookstoreUser> userManager,
                                           JwtTokenService jwtTokenService,
                                           ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new BookstoreUser { UserName = model.UserName };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation($"api/register {model.UserName} register successfully.");
                return Ok();
            }

            _logger.LogInformation($"api/register {model.UserName} register failed with {result.Errors}.");

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await _jwtTokenService.GenerateToken(user);

                _logger.LogInformation($"api/login {model.UserName} login successfully.");
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }
}
