using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using Xunit;
using Xunit.Abstractions;

namespace Neoflix.Challenges
{
    public class _09_GenreList : Neo4jChallengeTests
    {
        private readonly ITestOutputHelper _testOutput;

        public _09_GenreList(ITestOutputHelper output)
        {
            _testOutput = output;
        }

        [Fact]
        public async Task AllAsync_should_retrieve_list_of_genres()
        {
            var service = new GenreService(Neo4j.Driver);

            var output = await service.AllAsync();

            Assert.NotNull(output);
            Assert.Equal(19, output.Length);
            Assert.Equal("Action", output[0]["name"].As<string>());
            Assert.Equal("Western", output[18]["name"].As<string>());

            var topCount = output.OrderByDescending(x => x["movies"].As<int>()).First();

            _testOutput.WriteLine("Here is the answer to the quiz question on the lesson:");
            _testOutput.WriteLine("Which genre has the highest movie count?");
            _testOutput.WriteLine($"Copy and paste the following answer into the text box: {topCount["name"]}");
        }
    }
}
