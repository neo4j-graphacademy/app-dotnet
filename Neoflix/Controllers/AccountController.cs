using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neoflix.Services;

namespace Neoflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        /// <summary>
        /// </summary>
        /// <returns>the claims made in the JWT token</returns>
        [HttpGet]
        public async Task<IActionResult> GetClaimsAsync()
        {
            // todo: fix
            var user = HttpContext.User;

            var claims = user.Claims.ToArray();

            var result = claims.Select(x => new
            {
                name = x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                    ? "sub"
                    : x.Type,
                value = x.Value
            }).ToDictionary(x => x.name, x => x.value);

            return Ok(result);
        }

        // tag::list[]
        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavoritesAsync()
        {
            var (_, sort, order, limit, skip) = HttpRequestUtils
                .GetPagination(Request.Query, SortType.Movies);
            var userId = HttpRequestUtils.GetUserId(Request);

            var driver = Neo4j.Driver;
            var favoritesService = new FavoriteService(driver);

            var favorites = await favoritesService.AllAsync(userId, sort, order, limit, skip);

            return Ok(favorites);
        }
        // end::list[]

        // tag::add[]
        [HttpPost("favorites/{id}")]
        public async Task<IActionResult> AddFavoriteAsync(string id)
        {
            var userId = HttpRequestUtils.GetUserId(Request);

            var driver = Neo4j.Driver;
            var favoritesService = new FavoriteService(driver);

            var favorite = await favoritesService.AddAsync(userId, id);

            return Ok(favorite);
        }
        // end::add[]

        // tag::delete[]
        [HttpDelete("favorites/{id}")]
        public async Task<IActionResult> DeleteFavoriteAsync(string id)
        {
            var userId = HttpRequestUtils.GetUserId(Request);

            var driver = Neo4j.Driver;
            var favoritesService = new FavoriteService(driver);

            var favorite = await favoritesService.RemoveAsync(userId, id);

            return Ok(favorite);
        }
        // end::delete[]

        // tag::rating[]
        [HttpPost("ratings/{id}")]
        public async Task<IActionResult> AddRatingAsync(string id, [FromBody] int rating)
        {
            var userId = HttpRequestUtils.GetUserId(Request);

            var driver = Neo4j.Driver;
            var ratingService = new RatingService(driver);

            var rated = await ratingService.AddAsync(userId, id, rating);

            return Ok(rated);
        }
        // end::rating[]
    }
}