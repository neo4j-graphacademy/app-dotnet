using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neoflix.Services;
using NUnit.Framework;

namespace Neoflix.Challenges
{
    public class _05_Authentication : Neo4jChallengeTests
    {
        private const string Email = "authenticated@neo4j.com";
        private const string Password = "AuthenticateM3!";
        private const string Name = "Authenticated User";
        private bool[] successes = {false, false, false};

        public override async Task SetupAsync()
        {
            await base.SetupAsync();
            await using var session = Neo4j.Driver.AsyncSession();
            await session.ExecuteWriteAsync(tx =>
                tx.RunAsync("MATCH (u:User {email: $email}) DETACH DELETE u", new {email = Email}));
        }

        public override async Task TeardownAsync()
        {
            if (successes.All(x => x))
            {
                await using var session = Neo4j.Driver.AsyncSession();
                await session.ExecuteWriteAsync(tx => tx.RunAsync(@"
                    MATCH (u:User {email: $email})
                    SET u.authenticatedAt = datetime()", new { email = Email }));
            }

            await base.TeardownAsync();
        }

        [Test, Order(1)]
        public async Task AuthenticateAsync_should_authenticate__recently_created_user()
        {
            var service = new AuthService(Neo4j.Driver);

            await service.RegisterAsync(Email, Password, Name);

            var output = await service.AuthenticateAsync(Email, Password);

            Assert.AreEqual(Email, output["email"]);
            Assert.AreEqual(Name, output["name"]);
            Assert.Throws<KeyNotFoundException>(() =>
            {
                var value = output["password"];
            });
            Assert.False(string.IsNullOrEmpty(output["token"].ToString()));
            successes[0] = true;
        }

        [Test, Order(2)]
        public async Task AuthenticateAsync_should_return_false_on_incorrect_password()
        {
            var service = new AuthService(Neo4j.Driver);

            var result = await service.AuthenticateAsync(Email, "unknown");

            Assert.Null(result);
            successes[1] = true;
        }

        [Test, Order(3)]
        public async Task AuthenticateAsync_should_return_false_on_incorrect_email()
        {
            var service = new AuthService(Neo4j.Driver);

            var result = await service.AuthenticateAsync("unknown", Password);

            Assert.Null(result);
            successes[2] = true;

        }
    }
}
