using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Example;
using Neoflix.Exceptions;

namespace Neoflix.Services
{
    public class PeopleService
    {
        private readonly IDriver _driver;

        /// <summary>
        /// Initializes a new instance of <see cref="PeopleService"/> that handles Person database calls.
        /// </summary>
        /// <param name="driver">Instance of Neo4j Driver, which will be used to interact with Neo4j</param>
        public PeopleService(IDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// This method should return a paginated list of People (actors or directors), with an optional filter on the person.<br/><br/>
        /// Results should be filtered by query, ordered by the `sort` parameter and limited to limit, skip should be used to skip certain number of records.
        /// </summary>
        /// <param name="query">Optional query to filter by the persons name.</param>
        /// <param name="sort">Field in which to order the records.</param>
        /// <param name="order">Direction for the order.</param>
        /// <param name="limit">The maximum number of records to return.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains a list of records.
        /// </returns>
        // tag::all[]
        public async Task<Dictionary<string, object>[]> AllAsync(string? query, string sort = "name", Ordering order = Ordering.Asc, int limit = 6, int skip = 0)
        {
            // TODO: Get a list of people from the database
            return await Task.FromResult(Fixtures.People.Skip(skip).Take(limit).ToArray());
        }
        // end::all[]

        /// <summary>
        /// Find a user by their ID. <br/><br/>
        /// If no user is found, a NotFoundException should be thrown.
        /// </summary>
        /// <param name="id">The tmdbId for the user.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains user record.
        /// </returns>
        // tag::findById[]
        public async Task<Dictionary<string, object>> FindByIdAsync(string id)
        {
            // TODO: Find a user by their ID
            return await Task.FromResult(Fixtures.Pacino);
        }
        // end::findById[]

        /// <summary>
        /// Get a list of similar people to a Person, ordered by their similarity score in descending order.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <param name="limit">The maximum number of records to return.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.<br/>
        /// The task result contains list of records.
        /// </returns>
        // tag::getSimilarPeople[]
        public async Task<Dictionary<string, object>[]> GetSimilarPeopleAsync(string id, int limit = 6, int skip = 0)
        {
            // TODO: Get a list of similar people to the person by their id
            return await Task.FromResult(Fixtures.People.Skip(skip).Take(limit).ToArray());
        }
        // end::getSimilarPeople[]
    }
}