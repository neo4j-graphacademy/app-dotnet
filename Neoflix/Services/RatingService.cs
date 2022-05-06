using System.Collections.Generic;
using System.Linq;
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
            await using var session = _driver.AsyncSession();

            return await session.ExecuteReadAsync(async tx =>
            {
                var query = $@"
                    MATCH (u:User)-[r:RATED]->(m:Movie {{tmdbId: $id}})
                    RETURN r {{
                        .rating,
                        .timestamp,
                        user: u {{
                            .userId, .name
                        }}
                    }} AS review
                    ORDER BY r.{sort} {order.ToString("G").ToUpper()}
                    SKIP $skip
                    LIMIT $limit";
                var cursor = await tx.RunAsync(query, new {id, skip, limit});
                var records = await cursor.ToListAsync();

                return records
                    .Select(x => x["review"].As<Dictionary<string, object>>())
                    .ToArray();
            });
        }
        // end::forMovie[]

        /// <summary>
        /// Add a relationship between a User and a Movie with a "rating" property. <br/><br/>
        /// If the User or Movie cannot be found, a NotFoundException should be thrown
        /// </summary>
        /// <param name="userId">The userId for a user.</param>
        /// <param name="tmdbId">The tmdbId for a Movie.</param>
        /// <param name="rating">The rating from 1-5.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains a movie record with a rating property appended.
        /// </returns>
        // tag::add[]
        public async Task<Dictionary<string, object>> AddAsync(string userId, string tmdbId, int rating)
        {
            await using var session = _driver.AsyncSession();

            // tag::write[]
            var updatedMovie = await session.ExecuteWriteAsync(async tx =>
            {
                var cursor = await tx.RunAsync(@"
                    MATCH (u:User {userId: $userId})
                    MATCH (m:Movie {tmdbId: $tmdbId})

                    MERGE (u)-[r:RATED]->(m)
                    SET r.rating = $rating,
                        r.timestamp = timestamp()

                    RETURN m {
                        .*,
                        rating: r.rating
                    } as movie", new {userId, tmdbId, rating});

                if (!await cursor.FetchAsync())
                    return null;

                return cursor.Current["movie"].As<Dictionary<string, object>>();
            });
            // end::write[]

            // tag::throw[]
            if (updatedMovie == null)
                throw new NotFoundException($"Could not create rating for Movie: {tmdbId} for User: {userId}");
            // end::throw[]

            // tag::addreturn[]
            return updatedMovie;
            // end::addreturn[]
        }
        // end::add[]
    }
}
