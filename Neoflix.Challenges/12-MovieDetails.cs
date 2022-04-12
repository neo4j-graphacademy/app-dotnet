using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Neoflix.Challenges
{
    public class _12_MovieDetails : Neo4jChallengeTests
    {
        private const string LockStock = "100";
        private readonly ITestOutputHelper _testOutput;

        public _12_MovieDetails(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Fact]
        public async Task GetByIdAsync_should_get_a_movie_by_tmdbId()
        {
            var service = new MovieService(Neo4j.Driver);

            var output = await service.GetByIdAsync(LockStock);

            Assert.Equal(LockStock, output["tmdbId"].As<string>());
            Assert.Equal("Lock, Stock & Two Smoking Barrels", output["title"].As<string>());
        }

        [Fact]
        public async Task GetByIdAsync_should_get_similar_movies_ordered_by_similarity_score()
        {
            var service = new MovieService(Neo4j.Driver);

            var limit = 1;

            var output = await service.GetSimilarMoviesAsync(LockStock, limit, 0);
            var paginated = await service.GetSimilarMoviesAsync(LockStock, limit, 1);

            Assert.NotNull(output);
            Assert.Equal(limit, output.Length);

            Assert.NotEqual(JsonConvert.SerializeObject(output), 
                JsonConvert.SerializeObject(paginated));

            _testOutput.WriteLine("Here is the answer to the quiz question on the lesson:");
            _testOutput.WriteLine("What is the title of the most similar movie to Lock, Stock & Two Smoking Barrels?");
            _testOutput.WriteLine($"Copy and paste the following answer into the text box: {output[0]["title"]}");
        }
    }
}
