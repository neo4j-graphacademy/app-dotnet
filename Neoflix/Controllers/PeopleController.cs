using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neoflix.Services;

namespace Neoflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        /// <summary>
        /// Get a paginated list of People from the database
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            var (query, sort, order, limit, skip) = HttpRequestUtils
                .GetPagination(Request.Query, SortType.People);

            var driver = Neo4j.Driver;
            var peopleService = new PeopleService(driver);

            var people = await peopleService.AllAsync(query, sort, order, limit, skip);

            return Ok(people);
        }

        /// <summary>
        /// Get the properties of a Person based on their tmdbId
        /// </summary>
        /// <param name="id">Person's tmdbId.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var driver = Neo4j.Driver;
            var peopleService = new PeopleService(driver);

            var person = await peopleService.FindByIdAsync(id);

            return Ok(person);
        }

        /// <summary>
        /// Get a paginated list of similar people to the person with the id supplied in the route params.
        /// </summary>
        /// <param name="id">Person's tmdbId.</param>
        /// <returns></returns>
        [HttpGet("{id}/similar")]
        public async Task<IActionResult> GetSimilarAsync(string id)
        {
            var driver = Neo4j.Driver;
            var peopleService = new PeopleService(driver);

            var similarPeople = await peopleService.GetSimilarPeopleAsync(id);

            return Ok(similarPeople);
        }

        /// <summary>
        /// Get a paginated list of movies that the person with the id supplied has acted in.
        /// </summary>
        /// <param name="id">Person's tmdbId.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
        [HttpGet("{id}/acted")]
        public async Task<IActionResult> GetActedInAsync(string id)
        {
            var (_, sort, order, limit, skip) = HttpRequestUtils
                .GetPagination(Request.Query, SortType.Movies);
            var userId = HttpRequestUtils.GetUserId(Request);

            var driver = Neo4j.Driver;
            var movieService = new MovieService(driver);

            var movies = await movieService.GetForActorAsync(id, sort, order, limit, skip, userId);

            return Ok(movies);
        }

        /// <summary>
        /// Get a paginated list of movies that the person with the id supplied has acted in.
        /// </summary>
        /// <param name="id">Person's tmdbId.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains http result.
        /// </returns>
        [HttpGet("{id}/directed")]
        public async Task<IActionResult> GetDirectedAsync(string id)
        {
            var (_, sort, order, limit, skip) = HttpRequestUtils
                .GetPagination(Request.Query, SortType.Movies);
            var userId = HttpRequestUtils.GetUserId(Request);

            var driver = Neo4j.Driver;
            var movieService = new MovieService(driver);

            var movies = await movieService.GetForDirectorAsync(id, sort, order, limit, skip, userId);
            return Ok(movies);
        }
    }
}