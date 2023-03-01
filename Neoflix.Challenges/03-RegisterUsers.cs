using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Services;
using NUnit.Framework;

namespace Neoflix.Challenges
{
    public class _03_RegisterUsers : Neo4jChallengeTests
    {
        private const string Email = "graphacademy@neo4j.com";
        private const string Password = "letmein";
        private const string Name = "Graph Academy";

        public override async Task SetupAsync()
        {
            await base.SetupAsync();

            await using var session = Neo4j.Driver.AsyncSession();
            await session.ExecuteWriteAsync(tx =>
                tx.RunAsync("MATCH (u:User {email:$email}) DETACH DELETE u",
                    new {email = Email}));
        }

        [Test]
        public async Task RegisterAsync_should_register_a_user()
        {
            var service = new AuthService(Neo4j.Driver);

            var output = await service.RegisterAsync(Email, Password, Name);

            Assert.AreEqual(Email, output["email"]);
            Assert.AreEqual(Name, output["name"]);
            Assert.Throws<KeyNotFoundException>(() =>
            {
                var value = output["password"];
            });
            Assert.NotNull(output["token"]);

            await using var session = Neo4j.Driver.AsyncSession();
            var result = await session.ExecuteReadAsync(async tx =>
            {
                var cursor = await tx.RunAsync("MATCH (u:User {email:$email}) RETURN u",
                    new {email = Email});
                return await cursor.ToListAsync();
            });

            Assert.AreEqual(1, result.Count);
            var user = result[0].Values["u"].As<INode>();

            Assert.AreEqual(Email, user.Properties["email"].As<string>());
            Assert.AreEqual(Name, user.Properties["name"].As<string>());
            Assert.AreNotEqual(Password, user.Properties["password"].As<string>());
        }
    }
}