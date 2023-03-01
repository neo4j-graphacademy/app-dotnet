using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Exceptions;
using Neoflix.Services;
using NUnit.Framework;

namespace Neoflix.Challenges
{
    public class _07_FavoritesList : Neo4jChallengeTests
    {
        private const string ToyStory = "862";
        private const string Goodfellas = "769";
        private const string UserId = "9f965bf6-7e32-4afb-893f-756f502b2c2a";
        private const string Email = "graphacademy.favorite@neo4j.com";

        public override async Task SetupAsync()
        {
            await base.SetupAsync();
            await using var session = Neo4j.Driver.AsyncSession();
            await session.ExecuteWriteAsync(tx =>
                tx.RunAsync(@"
                    MERGE (u:User {userId: $userId})
                    SET u.email = $email",
                    new {userId = UserId, email = Email}));
        }

        [Test, Order(1)]
        public void AddAsync_should_throw_if_the_user_or_movie_do_not_exist()
        {
            var service = new FavoriteService(Neo4j.Driver);

            Assert.ThrowsAsync<NotFoundException>(async () => await service.AddAsync("unknown", "x999"));
        }

        [Test, Order(2)]
        public async Task AddAsync_should_save_movie_to_users_favorites()
        {
            var service = new FavoriteService(Neo4j.Driver);

            var output = await service.AddAsync(UserId, ToyStory);

            Assert.AreEqual(ToyStory, output["tmdbId"].As<string>());
            Assert.True(output["favorite"].As<bool>());

            var all = await service.AllAsync(UserId);

            Assert.True(all.Any(x => x["tmdbId"].As<string>() == ToyStory));
        }

        [Test, Order(3)]
        public async Task AddAsync_should_remove_movie_to_users_favorites()
        {
            var service = new FavoriteService(Neo4j.Driver);

            var output = await service.AddAsync(UserId, Goodfellas);

            Assert.AreEqual(Goodfellas, output["tmdbId"].As<string>());
            Assert.True(output["favorite"].As<bool>());

            var included = await service.AllAsync(UserId);

            Assert.True(included.Any(x => x["tmdbId"].As<string>() == Goodfellas));

            var removed = await service.RemoveAsync(UserId, Goodfellas);

            Assert.AreEqual(Goodfellas, removed["tmdbId"].As<string>());
            Assert.False(removed["favorite"].As<bool>());

            var excluded = await service.AllAsync(UserId);

            Assert.False(excluded.Any(x => x["tmdbId"].As<string>() == Goodfellas));
        }
    }
}
