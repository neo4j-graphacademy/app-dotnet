using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neoflix.Services;

namespace Neoflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            var driver = Neo4j.Driver;
            var genreService = new GenreService(driver);

            var genres = await genreService.AllAsync();

            return Ok(genres);
        }

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