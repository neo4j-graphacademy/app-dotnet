using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Example;
using Neoflix.Exceptions;

namespace Neoflix.Services
{
    public class RatingService
    {
        private readonly IDriver _driver;

        /// <summary>
        /// Initializes a new instance of <see cref="RatingService"/> that handles Rating database calls.
        /// </summary>
        /// <param name="driver">Instance of Neo4j Driver, which will be used to interact with Neo4j</param>
        public RatingService(IDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// Get a paginated list of reviews for a Movie. <br/><br/>
        /// Records should be ordered by <see cref="sort"/>, and in the direction specified by <see cref="order"/>. <br/>
        /// The maximum number of records returned should be limited by <see cref="limit"/> and <see cref="skip"/> should be used to skip a certain number of records.
        /// </summary>
        /// <param name="id">The tmdbId for a Movie.</param>
        /// <param name="sort">The field to order the records by.</param>
        /// <param name="order">The direction of the order.</param>
        /// <param name="limit">The maximum number of records to return.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains a list of records.
        /// </returns>
        // tag::forMovie[]
        public async Task<Dictionary<string, object>[]> GetForMovieAsync(string id, string sort = "timestamp",
            Ordering order = Ordering.Asc, int limit = 6, int skip = 0)
        {
            // TODO: Get ratings for a Movie
            return await Task.FromResult(Fixtures.Ratings);
        }
        // end::forMovie[]

        /// <summary>
        /// Add a relationship between a User and a Movie with a "rating" property. <br/><br/>
        /// If the User or Movie cannot be found, a NotFoundException should be thrown
        /// </summary>
        /// <param name="userId">The userId for a user.</param>
        /// <param name="movieId">The tmdbId for a Movie.</param>
        /// <param name="rating">The rating from 1-5.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains a movie record with a rating property appended.
        /// </returns>
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