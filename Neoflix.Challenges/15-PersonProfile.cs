using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Neoflix.Challenges
{
    public class _15_PersonProfile : Neo4jChallengeTests
    {
        private const string Coppola = "1776";

        [Test, Order(1)]
        public async Task FindByIdAsync_should_find_a_person_by_id()
        {
            var service = new PeopleService(Neo4j.Driver);

            var output = await service.FindByIdAsync(Coppola);

            Assert.NotNull(output);
            Assert.AreEqual(Coppola, output["tmdbId"].As<string>());
            Assert.AreEqual("Francis Ford Coppola", output["name"].As<string>());
            Assert.AreEqual(16, output["directedCount"].As<int>());
            Assert.AreEqual(2, output["actedCount"].As<int>());
        }

        [Test, Order(2)]
        public async Task GetSimilarPeopleAsync_should_return_paginated_list_of_similar_people()
        {
            var service = new PeopleService(Neo4j.Driver);

            var limit = 2;

            var output = await service.GetSimilarPeopleAsync(Coppola, limit);

            Assert.NotNull(output);
            Assert.AreEqual(limit, output.Length);

            var second = await service.GetSimilarPeopleAsync(Coppola, limit, limit);

            Assert.NotNull(second);
            Assert.AreEqual(limit, second.Length);
            Assert.AreNotEqual(JsonConvert.SerializeObject(output), JsonConvert.SerializeObject(second));

            var result = output[0]["name"].As<string>();

            TestContext.Out.WriteLine("Here is the answer to the quiz question on the lesson:");
            TestContext.Out.WriteLine("According to our algorithm, who is the most similar person to Francis Ford Coppola?");
            TestContext.Out.WriteLine($"Copy and paste the following answer into the text box: {result}");
        }
    }
}
