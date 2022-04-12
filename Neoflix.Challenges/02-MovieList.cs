using System.Linq;
using System.Threading.Tasks;
using Neoflix.Services;
using Xunit;
using Xunit.Abstractions;

namespace Neoflix.Challenges
{
    public class _02_MovieList : Neo4jChallengeTests
    {
        private readonly ITestOutputHelper _testOutput;

        public _02_MovieList(ITestOutputHelper output)
        {
            _testOutput = output;
        }

        [Fact]
        public async Task AllAsync_should_apply_order_skip_and_limit()
        {
            var service = new MovieService(Neo4j.Driver);

            var firstOutput = await service.AllAsync(limit: 1);

            Assert.NotNull(firstOutput);
            Assert.Single(firstOutput);

            var secondOutput = await service.AllAsync(limit: 1, skip: 1);

            Assert.NotNull(secondOutput);
            Assert.Single(secondOutput);

            var firstAsDict = firstOutput.First();
            var secondAsDict = secondOutput.First();

            Assert.NotEqual(firstAsDict["title"], secondAsDict["title"]);
        }

        [Fact]
        public async Task AllAsync_should_order_movies_by_rating()
        {
            var service = new MovieService(Neo4j.Driver);

            var output = await service.AllAsync("imdbRating", Ordering.Desc, limit: 1);

            Assert.NotNull(output);
            Assert.Single(output);

            var firstAsDict = output.First();

            _testOutput.WriteLine("Here is the answer to the quiz question on the lesson:");
            _testOutput.WriteLine("What is the title of the highest rated movie in the recommendations dataset?");
            _testOutput.WriteLine($"Copy and paste the following answer into the text box: {firstAsDict["title"]}\n");
        }
    }
}
