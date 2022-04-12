using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using Xunit;

namespace Neoflix.Challenges
{
    public class _03_RegisterUsers : Neo4jChallengeTests
    {
        private const string Email = "graphacademy@neo4j.com";
        private const string Password = "letmein";
        private const string Name = "Graph Academy";

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await using var session = Neo4j.Driver.AsyncSession();
            await session.WriteTransactionAsync(tx =>
                tx.RunAsync("MATCH (u:User {email:$email}) DETACH DELETE u",
                    new {email = Email}));
        }

        [Fact]
        public async Task RegisterAsync_should_register_a_user()
        {
            var service = new AuthService(Neo4j.Driver);

            var output = await service.RegisterAsync(Email, Password, Name);

            Assert.Equal(Email, output["email"]);
            Assert.Equal(Name, output["name"]);
            Assert.Throws<KeyNotFoundException>(() => output["password"]);
            Assert.NotNull(output["token"]);

            await using var session = Neo4j.Driver.AsyncSession();
            var result = await session.ReadTransactionAsync(async tx =>
            {
                var cursor = await tx.RunAsync("MATCH (u:User {email:$email}) DETACH DELETE u",
                    new {email = Email});
                return await cursor.ToListAsync();
            });

            Assert.Single(result);
            var user = result[0].Values["u"].As<INode>();

            Assert.Equal(Email, user.Properties["email"].As<string>());
            Assert.Equal(Name, user.Properties["name"].As<string>());
            Assert.NotEqual(Password, user.Properties["password"].As<string>());
        }
    }
}