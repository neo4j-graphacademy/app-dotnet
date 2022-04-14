using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using NUnit.Framework;

namespace Neoflix.Challenges
{
    public class _10_GenreDetails : Neo4jChallengeTests
    {
        [Test]
        public async Task FindAsync_should_retrieve_details_by_name()
        {
            var service = new GenreService(Neo4j.Driver);

            var output = await service.FindGenreAsync("Action");

            Assert.NotNull(output);
            Assert.AreEqual("Action", output["name"].As<string>());

            TestContext.Out.WriteLine("Here is the answer to the quiz question on the lesson:");
            TestContext.Out.WriteLine("How many movies are in the Action genre?");
            TestContext.Out.WriteLine($"Copy and paste the following answer into the text box: {output["movies"]}");
        }
    }
}
