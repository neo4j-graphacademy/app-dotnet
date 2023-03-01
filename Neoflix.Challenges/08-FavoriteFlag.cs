using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using NUnit.Framework;

namespace Neoflix.Challenges
{
    public class _08_FavoriteFlag : Neo4jChallengeTests
    {
        private const string UserId = "fe770c6b-4034-4e07-8e40-2f39e7a6722c";
        private const string Email = "graphacademy.flag@neo4j.com";

        public override async Task SetupAsync()
        {
            await base.SetupAsync();
            await using var session = Neo4j.Driver.AsyncSession();
            await session.ExecuteWriteAsync(tx =>
                tx.RunAsync(@"
                    MERGE (u:User {userId: $userId})
                    SET u.email = $email", new {userId = UserId, email = Email}));
        }

        [Test]
        public async Task AllAsync_should_return_true_for_all_items()
        {
            var movieService = new MovieService(Neo4j.Driver);
            var favoriteService = new FavoriteService(Neo4j.Driver);

            var first = (await movieService
                    .AllAsync("imdbRating", Ordering.Desc, limit: 1, userId: UserId))
                .First();

            var add = await favoriteService.AddAsync(UserId, first["tmdbId"].As<string>());

            Assert.AreEqual(first["tmdbId"].As<string>(), add["tmdbId"].As<string>());
            Assert.True(add["favorite"].As<bool>());

            var addCheck = await favoriteService.AllAsync(UserId, "imdbRating", limit: 999);
            Assert.True(addCheck.Any(x => x["tmdbId"].As<string>() == first["tmdbId"].As<string>()));

            var checks = await movieService.AllAsync("imdbRating", Ordering.Desc, limit: 2, userId: UserId);
            var checkFirst = checks[0];
            var checkSecond = checks[1];

            Assert.AreEqual(add["tmdbId"].As<string>(), checkFirst["tmdbId"].As<string>());
            Assert.True(checkFirst["favorite"].As<bool>());
            Assert.False(checkSecond["favorite"].As<bool>());
        }
    }
}
