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
        /// Get the claims made in the JWT token.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetClaimsAsync()
        {
            var user = HttpContext.User;

            var claims = user.Claims.ToArray();

            return await Task.FromResult(Ok(claims));
        }

        /// <summary>
        /// Get a list of movies that a user has added to their Favorites link by clicking the Bookmark icon on a Movie card.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
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

        /// <summary>
        /// Create a "HAS_FAVORITE" relationship between the current user and the movie with the id parameter.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
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

        /// <summary>
        /// Remove the "HAS_FAVORITE" relationship between the current user and the movie with the :id parameter.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
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

        /// <summary>
        /// Create a "RATING" relationship between the current user and the movie with the id parameter.<br/>
        /// The rating value will be posted as part of the post body.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="rating">Rating to add.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
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