using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Exceptions;
using Neoflix.Services;
using NUnit.Framework;

namespace Neoflix.Challenges
{
    public class _04_HandleConstraintErrors : Neo4jChallengeTests
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public override Task SetupAsync()
        {
            var random = new Random();
            Email = $"{random.Next()}@neo4j.com";
            Password = random.Next().ToString();
            Name = "Graph Academy";
            return base.SetupAsync();
        }

        public override async Task TeardownAsync()
        {
            await using (var session = Neo4j.Driver.AsyncSession())
                await session.ExecuteWriteAsync(tx =>
                        tx.RunAsync("MATCH (u:User {email:$email}) DETACH DELETE u",
                    new {email = Email}));

            await base.TeardownAsync();
        }

        [Test, Order(1)]
        public async Task Should_find_a_unique_constraint()
        {
            await using var session = Neo4j.Driver.AsyncSession();

            var result = await session.ExecuteReadAsync(async tx =>
                {
                    var cursor = await tx.RunAsync(@"
                        SHOW CONSTRAINTS
                        YIELD name, labelsOrTypes, properties
                        WHERE labelsOrTypes = ['User'] AND properties = ['email']
                        RETURN *");
                    return await cursor.ToListAsync();
                });

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [Test, Order(2)]
        public async Task Should_throw_a_ValidationException_when_email_already_exists()
        {
            var service = new AuthService(Neo4j.Driver);

            var output = await service.RegisterAsync(Email, Password, Name);

            Assert.AreEqual(Email, output["email"]);
            Assert.AreEqual(Name, output["name"]);
            Assert.Throws<KeyNotFoundException>(() =>
            {
                var value = output["password"];
            });
            Assert.False(string.IsNullOrEmpty(output["token"].ToString()));

            Assert.ThrowsAsync<ValidationException>(async () =>
                await service.RegisterAsync(Email, Password, Name));
        }
    }
}
