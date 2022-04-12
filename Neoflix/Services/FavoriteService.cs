using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Example;

namespace Neoflix.Services
{
    public class FavoriteService
    {
        private readonly IDriver _driver;

        public FavoriteService(IDriver driver)
        {
            _driver = driver;
        }

        // tag::all[]
        public async Task<Dictionary<string, object>[]> AllAsync(string userId, string sort = "title",
            Ordering order = Ordering.Asc, int limit = 6, int skip = 0)
        {
            // TODO: Open a new session
            // TODO: Retrieve a list of movies added to a user's favorites
            // TODO: Close session

            return await Task.FromResult(Fixtures.Popular);
        }
        // end::all[]

        // tag::add[]
        public Task<Dictionary<string, object>> AddAsync(string userId, string movieId)
        {
            // TODO: Open a new Session
            // TODO: Create HAS_FAVORITE relationship within a Write Transaction
            // TODO: Close the session
            // TODO: Return movie details and `favorite` property
            var data = Fixtures.Goodfellas
                .Concat(new[] {new KeyValuePair<string, object>("favorite", true)})
                .ToDictionary(x => x.Key, x => x.Value);

            return Task.FromResult(data);
        }
        // end::add[]

        // tag::remove[]
        public Task<Dictionary<string, object>> RemoveAsync(string userId, string movieId)
        {
            // TODO: Open a new Session
            // TODO: Delete the HAS_FAVORITE relationship within a Write Transaction
            // TODO: Close the session
            // TODO: Return movie details and `favorite` property

            var data = Fixtures.Goodfellas
                .Concat(new[] {new KeyValuePair<string, object>("favorite", true)})
                .ToDictionary(x => x.Key, x => x.Value);

            return Task.FromResult(data);
        }
        // end::remove[]
    }
}