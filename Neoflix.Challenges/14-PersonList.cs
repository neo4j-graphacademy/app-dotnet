using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neoflix.Challenges
{
    public class _14_PersonList : Neo4jChallengeTests
    {
        [Test, Order(1)]
        public async Task AllAsync_should_retrieve_paginated_list_from_database()
        {
            var service = new PeopleService(Neo4j.Driver);

            var limit = 10;

            var output = await service.AllAsync(null, "name", Ordering.Asc, limit);

            Assert.NotNull(output);
            Assert.AreEqual(limit, output.Length);

            var paginated = await service.AllAsync(null, "name", Ordering.Asc, limit, limit);

            Assert.NotNull(paginated);
            Assert.AreEqual(limit, paginated.Length);

            Assert.AreNotEqual(JsonConvert.SerializeObject(output), JsonConvert.SerializeObject(paginated));
        }

        [Test, Order(2)]
        public async Task AllAsync_should_apply_filter_order_pagination_to_query()
        {
            var service = new PeopleService(Neo4j.Driver);

            var query = "A";

            var first = await service.AllAsync(query, "name", Ordering.Asc, 1);
            var last = await service.AllAsync(query, "name", Ordering.Desc, 1);

            Assert.NotNull(first);
            Assert.AreEqual(1, first.Length);

            Assert.NotNull(last);
            Assert.AreEqual(1, last.Length);

            Assert.AreNotEqual(JsonConvert.SerializeObject(first), JsonConvert.SerializeObject(last));
        }

        [Test, Order(3)]
        public async Task AllAsync_should_apply_filter_order_pagination()
        {
            var service = new PeopleService(Neo4j.Driver);

            var first = await service.AllAsync(null, "name", Ordering.Asc, 1);
            var result = first[0]["name"].As<string>();

            TestContext.Out.WriteLine("Here is the answer to the quiz question on the lesson:");
            TestContext.Out.WriteLine("What is the name of the first person in the database in alphabetical order?");
            TestContext.Out.WriteLine($"Copy and paste the following answer into the text box: {result}");
        }
    }
}
