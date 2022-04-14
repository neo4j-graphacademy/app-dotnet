using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using NUnit.Framework;

namespace Neoflix.Challenges
{
    public class _11_MovieLists : Neo4jChallengeTests
    {
        private const string TomHanks = "31";
        private const string Coppola = "1776";

        [Test, Order(1)]
        public async Task GetByGenreAsync_should_return_paginated_list_of_movies_by_genre()
        {
            var service = new MovieService(Neo4j.Driver);

            var genre = "Comedy";
            var limit = 10;

            var output = await service.GetByGenreAsync(genre, "title", 
                Ordering.Asc, limit: limit);

            Assert.NotNull(output);
            Assert.AreEqual(limit, output.Length);

            var secondOutput = await service.GetByGenreAsync(genre, "title",
                Ordering.Asc, limit: limit, skip: limit);

            Assert.NotNull(secondOutput);
            Assert.AreEqual(limit, secondOutput.Length);

            Assert.AreNotEqual(output[0]["title"].As<string>(), 
                secondOutput[0]["title"].As<string>());

            var reordered = await service.GetByGenreAsync(genre, "released",
                Ordering.Asc, limit, limit);

            Assert.AreNotEqual(reordered[0]["title"].As<string>(),
                output[0]["title"].As<string>());
        }

        [Test, Order(2)]
        public async Task GetForActorAsync_should_return_paginated_list()
        {
            var service = new MovieService(Neo4j.Driver);

            var limit = 2;

            var output = await service.GetForActorAsync(TomHanks, "title",
                Ordering.Asc, limit, 0);

            Assert.NotNull(output);
            Assert.AreEqual(limit, output.Length);

            var secondOutput = await service.GetForActorAsync(TomHanks, "title",
                Ordering.Asc, limit, limit);

            Assert.NotNull(output);
            Assert.AreEqual(limit, secondOutput.Length);

            Assert.AreNotEqual(output[0]["title"].As<string>(),
                secondOutput[0]["title"].As<string>());

            var reordered = await service.GetForActorAsync(TomHanks, "released",
                Ordering.Asc, limit, 0);

            Assert.AreNotEqual(output[0]["title"].As<string>(),
                reordered[0]["title"].As<string>());
        }

        [Test, Order(3)]
        public async Task GetForDirectorAsync_should_return_paginated_list()
        {
            var service = new MovieService(Neo4j.Driver);

            var limit = 1;

            var output = await service.GetForDirectorAsync(TomHanks, "title",
                Ordering.Asc, limit, 0);

            Assert.NotNull(output);
            Assert.AreEqual(limit, output.Length);

            var secondOutput = await service.GetForDirectorAsync(TomHanks, "title",
                Ordering.Asc, limit, limit);

            Assert.NotNull(output);
            Assert.AreEqual(limit, secondOutput.Length);

            Assert.AreNotEqual(output[0]["title"].As<string>(),
                secondOutput[0]["title"].As<string>());

            var reordered = await service.GetForDirectorAsync(TomHanks, "released",
                Ordering.Asc, limit, 0);

            Assert.AreNotEqual(output[0]["title"].As<string>(),
                reordered[0]["title"].As<string>());
        }

        [Test, Order(4)]
        public async Task GetForDirectorAsync_should_find_films_by_Francis_Ford_Coppola()
        {
            var service = new MovieService(Neo4j.Driver);

            var output = await service.GetForDirectorAsync(Coppola, "imdbRating",
                Ordering.Desc, 30);
            Assert.AreEqual(16, output.Length);

            TestContext.Out.WriteLine("Here is the answer to the quiz question on the lesson:");
            TestContext.Out.WriteLine("How many films has Francis Ford Coppola directed?");
            TestContext.Out.WriteLine($"Copy and paste the following answer into the text box: {output.Length}");
        }
    }
}
