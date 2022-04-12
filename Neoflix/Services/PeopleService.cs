using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Example;

namespace Neoflix.Services
{
    public class PeopleService
    {
        private readonly IDriver _driver;

        public PeopleService(IDriver driver)
        {
            _driver = driver;
        }

        // tag::all[]
        public async Task<Dictionary<string, object>[]> AllAsync(string query, string sort, Ordering order, int limit = 6, int skip = 0)
        {
            // TODO: Get a list of people from the database
            return await Task.FromResult(Fixtures.People.Skip(skip).Take(limit).ToArray());
        }
        // end::all[]

        // tag::findById[]
        public async Task<Dictionary<string, object>> FindByIdAsync(string id)
        {
            // TODO: Find a user by their ID
            return await Task.FromResult(Fixtures.Pacino);
        }
        // end::findById[]

        // tag::getSimilarPeople[]
        public async Task<Dictionary<string, object>[]> GetSimilarPeopleAsync(string id, int limit = 6, int skip = 0)
        {
            // TODO: Get a list of similar people to the person by their id
            return await Task.FromResult(Fixtures.People.Skip(skip).Take(limit).ToArray());
        }
        // end::getSimilarPeople[]
    }
}