using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neoflix.Services;

namespace Neoflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// invokes the `Neo4jStrategy` in `src/passport/neo4j.strategy.js`, <br/>
        /// which, when implemented, attempts to authenticate the user against the Neo4j database.
        /// </summary>
        /// <param name="loginDto">Login data.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
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

        /// <summary>
        /// Create a new User node in the database with an encrypted password before returning a User record which includes a "token" property.
        /// This token is then used 
        /// </summary>
        /// <param name="registerDto">Registration data.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
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