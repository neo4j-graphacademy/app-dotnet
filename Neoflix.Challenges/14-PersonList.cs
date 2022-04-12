using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Neoflix.Challenges
{
    public class _14_PersonList : Neo4jChallengeTests
    {
        private readonly ITestOutputHelper _testOutput;

        public _14_PersonList(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Fact]
        public async Task AllAsync_should_retrieve_paginated_list_from_database()
        {
            var service = new PeopleService(Neo4j.Driver);

            var limit = 10;

            var output = await service.AllAsync(null, "name", Ordering.Asc, limit);

            Assert.NotNull(output);
            Assert.Equal(limit, output.Length);

            var paginated = await service.AllAsync(null, "name", Ordering.Asc, limit, limit);

            Assert.NotNull(paginated);
            Assert.Equal(limit, paginated.Length);

            Assert.NotEqual(JsonConvert.SerializeObject(output), JsonConvert.SerializeObject(paginated));
        }

        [Fact]
        public async Task AllAsync_should_apply_filter_order_pagination_to_query()
        {
            var service = new PeopleService(Neo4j.Driver);

            var query = "A";

            var first = await service.AllAsync(query, "name", Ordering.Asc, 1);
            var last = await service.AllAsync(query, "name", Ordering.Desc, 1);

            Assert.NotNull(first);
            Assert.Single(first);

            Assert.NotNull(last);
            Assert.Single(last);

            Assert.NotEqual(JsonConvert.SerializeObject(first), JsonConvert.SerializeObject(last));
        }

        [Fact]
        public async Task AllAsync_should_apply_filter_order_pagination()
        {
            var service = new PeopleService(Neo4j.Driver);

            var first = await service.AllAsync(null, "name", Ordering.Asc, 1);
            var result = first[0]["name"].As<string>();

            _testOutput.WriteLine("Here is the answer to the quiz question on the lesson:");
            _testOutput.WriteLine("What is the name of the first person in the database in alphabetical order?");
            _testOutput.WriteLine($"Copy and paste the following answer into the text box: {result}");
        }
    }
}
