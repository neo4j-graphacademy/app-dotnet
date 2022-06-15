using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neoflix.Services;

namespace Neoflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        /// <summary>
        /// Get a paginated list of movies, sorted by the "sort" query parameter,
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
        // tag::list[]
        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            var (_, sort, order, limit, skip) = HttpRequestUtils
                .GetPagination(Request.Query, SortType.Movies);
            var userId = HttpRequestUtils.GetUserId(Request);

            var driver = Neo4j.Driver;
            var movieService = new MovieService(driver);

            var movies = await movieService.AllAsync(sort, order, limit, skip, userId);

            return Ok(movies);
        }
        // end::list[]

        /// <summary>
        /// Find a movie by its tmdbId and return its properties.
        /// </summary>
        /// <param name="id">Movie's tmdbId.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
        // tag::get[]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var driver = Neo4j.Driver;
            var movieService = new MovieService(driver);
            var userId = HttpRequestUtils.GetUserId(Request);

            var movies = await movieService.FindByIdAsync(id, userId);

            return Ok(movies);
        }
        // end::get[]

        /// <summary>
        /// Get a paginated list of ratings for a movie, ordered by either the rating itself or when the review was created.
        /// </summary>
        /// <param name="id">Movie's tmdbId.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
        // tag::ratings[]
        [HttpGet("{id}/ratings")]
        public async Task<IActionResult> GetRatingsById(string id)
        {
            var (_, sort, order, limit, skip) = HttpRequestUtils
                .GetPagination(Request.Query, SortType.Rating);

            var driver = Neo4j.Driver;
            var ratingService = new RatingService(driver);

            var reviews = await ratingService.GetForMovieAsync(id, sort, order, limit, skip);

            return Ok(reviews);
        }
        // end::ratings[]

        /// <summary>
        /// Get a paginated list of similar movies, ordered by the similarity score in descending order.
        /// </summary>
        /// <param name="id">Movie's tmdbId.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
        // tag::similar[]
        [HttpGet("{id}/similar")]
        public async Task<IActionResult> GetSimilarByAsync(string id)
        {
            var (_, _, _, limit, skip) = HttpRequestUtils
                .GetPagination(Request.Query);

            var driver = Neo4j.Driver;
            var movieService = new MovieService(driver);

            var movies = await movieService.GetSimilarMoviesAsync(id, limit, skip);

            if (movies == null)
                return NotFound($"Movie with id {id} not found.");

            return Ok(movies);
        }
        // end::similar[]
    }
}