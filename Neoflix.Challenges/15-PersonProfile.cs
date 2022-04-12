using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Neoflix.Challenges
{
    public class _15_PersonProfile : Neo4jChallengeTests
    {
        private const string Coppola = "1776";
        private readonly ITestOutputHelper _testOutput;

        public _15_PersonProfile(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Fact]
        public async Task FindByIdAsync_should_find_a_person_by_id()
        {
            var service = new PeopleService(Neo4j.Driver);

            var output = await service.FindByIdAsync(Coppola);

            Assert.NotNull(output);
            Assert.Equal(Coppola, output["tmdbId"].As<string>());
            Assert.Equal("Francis Ford Coppola", output["name"].As<string>());
            Assert.Equal(16, output["directedCount"].As<int>());
            Assert.Equal(2, output["actedCount"].As<int>());
        }

        [Fact]
        public async Task GetSimilarPeopleAsync_should_return_paginated_list_of_similar_people()
        {
            var service = new PeopleService(Neo4j.Driver);

            var limit = 2;

            var output = await service.GetSimilarPeopleAsync(Coppola, limit);

            Assert.NotNull(output);
            Assert.Equal(limit, output.Length);

            var second = await service.GetSimilarPeopleAsync(Coppola, limit, limit);

            Assert.NotNull(second);
            Assert.Equal(limit, second.Length);
            Assert.NotEqual(JsonConvert.SerializeObject(output), JsonConvert.SerializeObject(second));

            var result = output[0]["name"].As<string>();

            _testOutput.WriteLine("Here is the answer to the quiz question on the lesson:");
            _testOutput.WriteLine("According to our algorithm, who is the most similar person to Francis Ford Coppola?");
            _testOutput.WriteLine($"Copy and paste the following answer into the text box: {result}");
        }
    }
}
