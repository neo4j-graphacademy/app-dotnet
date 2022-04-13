using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Example;
using Neoflix.Exceptions;

namespace Neoflix.Services
{
    public class FavoriteService
    {
        private readonly IDriver _driver;

        /// <summary>
        /// Initializes a new instance of <see cref="FavoriteService"/> that handles favorite database calls.
        /// </summary>
        /// <param name="driver">Instance of Neo4j Driver, which will be used to interact with Neo4j</param>
        public FavoriteService(IDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// Get a list of movies that have an incoming "HAS_FAVORITE" relationship from a User node with the supplied "userId".<br/><br/>
        /// Records should be ordered by <see cref="sort"/>, and in the direction specified by <see cref="order"/>. <br/>
        /// The maximum number of records returned should be limited by <see cref="limit"/> and <see cref="skip"/> should be used to skip a certain number of records.<br/><br/>
        /// </summary>
        /// <param name="userId">The User's Id.</param>
        /// <param name="sort">The field to order the records by.</param>
        /// <param name="order">The direction of the order.</param>
        /// <param name="limit">The maximum number of records to return.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains a list of records.
        /// </returns>
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

        /// <summary>
        /// Create a "HAS_FAVORITE" relationship between the User and Movie ID nodes provided. <br/><br/>
        /// If either the user or movie cannot be found, a `NotFoundError` should be thrown.
        /// </summary>
        /// <param name="userId">The unique ID for the User node.</param>
        /// <param name="movieId">The unique tmdbId for the Movie node.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains The updated movie record with `favorite` set to true.
        /// </returns>
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

        /// <summary>
        /// Remove the "HAS_FAVORITE" relationship between the User and Movie ID nodes provided. <br/><br/>
        /// If either the user or movie cannot be found, a `NotFoundError` should be thrown.
        /// </summary>
        /// <param name="userId">The unique ID for the User node.</param>
        /// <param name="movieId">The unique tmdbId for the Movie node.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains The updated movie record with `favorite` set to false.
        /// </returns>
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