using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neoflix.Challenges
{
    public class _12_MovieDetails : Neo4jChallengeTests
    {
        private const string LockStock = "100";

        [Test, Order(1)]
        public async Task GetByIdAsync_should_get_a_movie_by_tmdbId()
        {
            var service = new MovieService(Neo4j.Driver);

            var output = await service.FindByIdAsync(LockStock);

            Assert.AreEqual(LockStock, output["tmdbId"].As<string>());
            Assert.AreEqual("Lock, Stock & Two Smoking Barrels", output["title"].As<string>());
        }

        [Test, Order(2)]
        public async Task GetByIdAsync_should_get_similar_movies_ordered_by_similarity_score()
        {
            var service = new MovieService(Neo4j.Driver);

            var limit = 1;

            var output = await service.GetSimilarMoviesAsync(LockStock, limit, 0);
            var paginated = await service.GetSimilarMoviesAsync(LockStock, limit, 1);

            Assert.NotNull(output);
            Assert.AreEqual(limit, output.Length);

            Assert.AreNotEqual(JsonConvert.SerializeObject(output), 
                JsonConvert.SerializeObject(paginated));

            TestContext.Out.WriteLine("Here is the answer to the quiz question on the lesson:");
            TestContext.Out.WriteLine("What is the title of the most similar movie to Lock, Stock & Two Smoking Barrels?");
            TestContext.Out.WriteLine($"Copy and paste the following answer into the text box: {output[0]["title"]}");
        }
    }
}
