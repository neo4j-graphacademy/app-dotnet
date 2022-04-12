using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Example;

namespace Neoflix.Services
{
    public class GenreService
    {
        private readonly IDriver _driver;

        public GenreService(IDriver driver)
        {
            _driver = driver;
        }

        // tag::all[]
        public Task<Dictionary<string, object>[]> AllAsync()
        {
            // TODO: Open a new session
            // TODO: Get a list of Genres from the database
            // TODO: Close the session

            return Task.FromResult(Fixtures.Genres.ToArray());
        }
        // end::all[]

        // tag::find[]
        public Task<Dictionary<string, object>> FindGenreAsync(string name)
        {
            // TODO: Open a new session
            // TODO: Get Genre information from the database
            // TODO: return null if the genre is not found
            // TODO: Close the session

            return Task.FromResult(
                Fixtures
                    .Genres
                    .OfType<Dictionary<string, object>>()
                    .First(x => x["name"] == name));
        }
        // end::find[]
    }
}