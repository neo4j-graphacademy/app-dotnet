using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neoflix.Services;

namespace Neoflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        // tag::list[]
        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            var (_, sort, order, limit, skip) = HttpRequestUtils
                .GetPagination(Request.Query, SortType.Movies);

            var driver = Neo4j.Driver;
            var movieService = new MovieService(driver);

            var movies = await movieService.AllAsync(sort, order, limit, skip);

            return Ok(movies);
        }
        // end::list[]

        // tag::get[]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var driver = Neo4j.Driver;
            var movieService = new MovieService(driver);

            var movies = await movieService.GetByIdAsync(id);

            return Ok(movies);
        }
        // end::get[]

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