using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using Xunit;
using Xunit.Abstractions;

namespace Neoflix.Challenges
{
    public class _10_GenreDetails : Neo4jChallengeTests
    {
        private readonly ITestOutputHelper _testOutput;

        public _10_GenreDetails(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Fact]
        public async Task FindAsync_should_retrieve_details_by_name()
        {
            var service = new GenreService(Neo4j.Driver);

            var output = await service.FindGenreAsync("Action");

            Assert.NotNull(output);
            Assert.Equal("Action", output["name"].As<string>());

            _testOutput.WriteLine("Here is the answer to the quiz question on the lesson:");
            _testOutput.WriteLine("How many movies are in the Action genre?");
            _testOutput.WriteLine($"Copy and paste the following answer into the text box: {output["movies"]}");
        }
    }
}
