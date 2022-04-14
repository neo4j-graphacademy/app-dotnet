using System.Linq;
using System.Threading.Tasks;
using Neoflix.Services;
using NUnit.Framework;

namespace Neoflix.Challenges
{
    public class _02_MovieList : Neo4jChallengeTests
    {
        [Test, Order(1)]
        public async Task AllAsync_should_apply_order_skip_and_limit()
        {
            var service = new MovieService(Neo4j.Driver);

            var firstOutput = await service.AllAsync(limit: 1);

            Assert.NotNull(firstOutput);
            Assert.AreEqual(1, firstOutput.Length);

            var secondOutput = await service.AllAsync(limit: 1, skip: 1);

            Assert.NotNull(secondOutput);
            Assert.AreEqual(1, secondOutput.Length);

            var firstAsDict = firstOutput.First();
            var secondAsDict = secondOutput.First();

            Assert.AreNotEqual(firstAsDict["title"], secondAsDict["title"]);
        }

        [Test, Order(2)]
        public async Task AllAsync_should_order_movies_by_rating()
        {
            var service = new MovieService(Neo4j.Driver);

            var output = await service.AllAsync("imdbRating", Ordering.Desc, limit: 1);

            Assert.NotNull(output);
            Assert.AreEqual(1, output.Length);

            var firstAsDict = output.First();

            TestContext.Out.WriteLine("Here is the answer to the quiz question on the lesson:");
            TestContext.Out.WriteLine("What is the title of the highest rated movie in the recommendations dataset?");
            TestContext.Out.WriteLine($"Copy and paste the following answer into the text box: {firstAsDict["title"]}\n");
        }
    }
}
