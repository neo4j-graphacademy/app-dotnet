using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using Xunit;

namespace Neoflix.Challenges
{
    public class _06_RatingMovies : Neo4jChallengeTests
    {
        private const string MovieId = "769";
        private const string UserId = "1185150b-9e81-46a2-a1d3-eb649544b9c4";
        private const string Email = "graphacademy.reviewer@neo4j.com";
        private const int Rating = 5;

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await using var session = Neo4j.Driver.AsyncSession();
            await session.WriteTransactionAsync(tx =>
                tx.RunAsync(@"
                    MERGE (u:User {userId: $userId})
                    SET u.email = $email", 
                    new {userId = UserId, email = Email}));
        }

        [Fact]
        public async Task AddAsync_should_store_rating_as_integer()
        {
            var service = new RatingService(Neo4j.Driver);

            var output = await service.AddAsync(UserId, MovieId, Rating);

            Assert.Equal(MovieId, output["tmdbId"]);
            Assert.Equal(Rating, output["rating"].As<int>());
        }
    }
}
