using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using NUnit.Framework;

namespace Neoflix.Challenges
{
    public class _09_GenreList : Neo4jChallengeTests
    {
        [Test]
        public async Task AllAsync_should_retrieve_list_of_genres()
        {
            var service = new GenreService(Neo4j.Driver);

            var output = await service.AllAsync();

            Assert.NotNull(output);
            Assert.AreEqual(19, output.Length);
            Assert.AreEqual("Action", output[0]["name"].As<string>());
            Assert.AreEqual("Western", output[18]["name"].As<string>());

            var topCount = output.OrderByDescending(x => x["movies"].As<int>()).First();

            TestContext.Out.WriteLine("Here is the answer to the quiz question on the lesson:");
            TestContext.Out.WriteLine("Which genre has the highest movie count?");
            TestContext.Out.WriteLine($"Copy and paste the following answer into the text box: {topCount["name"]}");
        }
    }
}
