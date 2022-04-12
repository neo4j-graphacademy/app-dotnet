using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neoflix.Services;

namespace Neoflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // tag::login[]
        [HttpPost("login")]
        public async Task<object> LoginAsync([FromBody]LoginDto loginDto)
        {
            var driver = Neo4j.Driver;
            var authService = new AuthService(driver);

            var user = await authService.AuthenticateAsync(loginDto.Email, loginDto.Password);

            if (user == null)
                return Unauthorized();

            return Ok(user);
        }
        // end::login[]

        // tag::register[]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            var driver = Neo4j.Driver;
            var authService = new AuthService(driver);

            var user = await authService.RegisterAsync(registerDto.Email, registerDto.Password, registerDto.Name);

            return Ok(user);
        }
        // end:register[]
    }

    public class LoginDto
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }

    public class RegisterDto
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string Name { get; init; }
    }
}