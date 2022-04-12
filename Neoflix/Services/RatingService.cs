using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Example;

namespace Neoflix.Services
{
    public class RatingService
    {
        private readonly IDriver _driver;

        public RatingService(IDriver driver)
        {
            _driver = driver;
        }

        // tag::forMovie[]
        public async Task<Dictionary<string, object>[]> GetForMovieAsync(string id, string sort = "timestamp",
            Ordering order = Ordering.Asc, int limit = 6, int skip = 0)
        {
            // TODO: Get ratings for a Movie
            return await Task.FromResult(Fixtures.Ratings);
        }
        // end::forMovie[]

        // tag::add[]
        public async Task<Dictionary<string, object>> AddAsync(string userId, string movieId, int rating)
        {
            // TODO: Convert the native integer into a Neo4j Integer
            // TODO: Save the rating in the database
            // TODO: Return movie details and a rating

            return await Task.FromResult(Fixtures.Goodfellas);
        }
        // end::add[]
    }
}