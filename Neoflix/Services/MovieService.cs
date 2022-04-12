using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Example;

namespace Neoflix.Services
{
    public class MovieService
    {
        private readonly IDriver _driver;

        public MovieService(IDriver driver)
        {
            _driver = driver;
        }

        // tag::all[]
        public async Task<Dictionary<string, object>[]> AllAsync(string sort = "title", 
            Ordering order = Ordering.Asc, int limit = 6, int skip = 0, string userId = null)
        {
            // TODO: Open an Session
            // TODO: Execute a query in a new Read Transaction
            // TODO: Get a list of Movies from the Result
            // TODO: Close the session

            return await Task.FromResult(Fixtures.Popular.Skip(skip).Take(limit).ToArray());
        }
        // end::all[]

        // tag::getByGenre[]
        public async Task<Dictionary<string, object>[]> GetByGenreAsync(string name, string sort = "title",
            Ordering order = Ordering.Asc, int limit = 6, int skip = 0, string userId = null)
        {
            // TODO: Get Movies in a Genre
            // MATCH (m:Movie)-[:IN_GENRE]->(:Genre {name: $name})

            return await Task.FromResult(Fixtures.Genres.Skip(skip).Take(limit).ToArray());
        }
        // end::getByGenre[]

        // tag::getForActor[]
        public async Task<Dictionary<string, object>[]> GetForActorAsync(string id, string sort = "title",
            Ordering order = Ordering.Asc, int limit = 6, int skip = 0, string userId = null)
        {
            // TODO: Get Movies acted in by a Person
            // MATCH (:Person {tmdbId: $id})-[:ACTED_IN]->(m:Movie)

            return await Task.FromResult(Fixtures.Roles.Skip(skip).Take(limit).ToArray());
        }
        // end::getForActor[]

        // tag::getForDirector[]
        public async Task<Dictionary<string, object>[]> GetForDirectorAsync(string id, string sort = "title",
            Ordering order = Ordering.Asc, int limit = 6, int skip = 0, string userId = null)
        {
            // TODO: Get Movies directed by a Person
            // MATCH (:Person {tmdbId: $id})-[:DIRECTED]->(m:Movie)

            return await Task.FromResult(Fixtures.Popular.Skip(skip).Take(limit).ToArray());
        }
        // end::getForDirector[]

        // tag::getById[]
        public async Task<Dictionary<string, object>> GetByIdAsync(string id)
        {
            // TODO: Find a movie by its ID
            // MATCH (m:Movie {tmdbId: $id})

            return await Task.FromResult(Fixtures.Goodfellas);
        }
        // end:getById[]

        // tag::getSimilarMovies[]
        public async Task<Dictionary<string, object>[]> GetSimilarMoviesAsync(string id, int limit, int skip)
        {
            // TODO: Get similar movies based on genres or ratings
            var random = new Random();
            var exampleData = Fixtures.Popular
                .Skip(skip)
                .Take(limit)
                .Select(popularItem =>
                    popularItem.Concat(new[]
                        {
                            new KeyValuePair<string, object>("score", Math.Round(random.NextDouble() * 100, 2))
                        })
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
                .ToArray();
            return await Task.FromResult(exampleData);
            // end::getSimilarMovies[]
        }

        // tag::getUserFavorites[]
        private async Task<string[]> GetUserFavoritesAsync(IAsyncTransaction transaction, string userId)
        {
            return await Task.FromResult(Array.Empty<string>());
        }
        // end::getUserFavorites[]
    }
}