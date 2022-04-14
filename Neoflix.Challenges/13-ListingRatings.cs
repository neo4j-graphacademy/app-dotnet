using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neoflix.Challenges
{
    public class _13_ListingRatings : Neo4jChallengeTests
    {
        private const string PulpFiction = "680";

        [Test, Order(1)]
        public async Task GetForMovieAsync_should_retrieve_a_list_of_ratings()
        {
            var service = new RatingService(Neo4j.Driver);

            var limit = 10;

            var output = await service.GetForMovieAsync(PulpFiction, "timestamp",
                Ordering.Desc, limit);

            Assert.NotNull(output);
            Assert.AreEqual(limit, output.Length);

            var paginated = await service.GetForMovieAsync(PulpFiction, "timestamp",
                Ordering.Desc, limit, limit);

            Assert.NotNull(paginated);
            Assert.AreEqual(limit, paginated.Length);

            Assert.AreNotEqual(JsonConvert.SerializeObject(paginated),
                JsonConvert.SerializeObject(output));
        }

        [Test, Order(2)]
        public async Task GetForMovieAsync_should_apply_ordering_and_pagination_to_query()
        {
            var service = new RatingService(Neo4j.Driver);

            var first = await service.GetForMovieAsync(PulpFiction, "timestamp", Ordering.Asc, 1);
            var latest = await service.GetForMovieAsync(PulpFiction, "timestamp", Ordering.Desc, 1);

            Assert.AreNotEqual(JsonConvert.SerializeObject(first),
                JsonConvert.SerializeObject(latest));

            var result = first[0]
                .As<Dictionary<string, object>>()["user"]
                .As<Dictionary<string, object>>()["name"];

            TestContext.Out.WriteLine("Here is the answer to the quiz question on the lesson:");
            TestContext.Out.WriteLine("What is the name of the first person to rate the movie Pulp Fiction?");
            TestContext.Out.WriteLine($"Copy and paste the following answer into the text box: {result}");

        }
    }
}
