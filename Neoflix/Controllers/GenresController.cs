using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neoflix.Services;

namespace Neoflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        /// <summary>
        /// Get a full list of Genres from the database along with a poster and movie count.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            var driver = Neo4j.Driver;
            var genreService = new GenreService(driver);

            var genres = await genreService.AllAsync();

            return Ok(genres);
        }

        /// <summary>
        /// Get information on a genre with a name that matches the name URL parameter.<br/>
        /// If the genre is not found, a 404 should be thrown.
        /// </summary>
        /// <param name="name">Genre name.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
        [HttpGet("{name}")]
        public async Task<IActionResult> GetGenreByNameAsync(string name)
        {
            var driver = Neo4j.Driver;
            var genreService = new GenreService(driver);

            var genre = await genreService.FindGenreAsync(name);

            if (genre == null)
                return NotFound();

            return Ok(genre);
        }

        /// <summary>
        /// Get a paginated list of movies that are listed in the genre whose name matches the name URL parameter.
        /// </summary>
        /// <param name="name">Genre name.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
        [HttpGet("{name}/movies")]
        public async Task<IActionResult> GetMoviesByGenreAsync(string name)
        {
            var (_, sort, order, limit, skip) = HttpRequestUtils
                .GetPagination(Request.Query, SortType.Movies);
            var userId = HttpRequestUtils.GetUserId(Request);

            var driver = Neo4j.Driver;
            var genreService = new MovieService(driver);

            var movies = await genreService.GetByGenreAsync(name, sort, order, limit, skip, userId);
            return Ok(movies);
        }
    }
}