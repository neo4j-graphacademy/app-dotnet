using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;
using Neoflix.Exceptions;
using Neoflix.Services;
using Xunit;

namespace Neoflix.Challenges
{
    public class _04_HandleConstraintErrors : Neo4jChallengeTests
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public override Task InitializeAsync()
        {
            var random = new Random();
            Email = $"{random.Next()}@neo4j.com";
            Password = random.Next().ToString();
            Name = "Graph Academy";
            return base.InitializeAsync();
        }

        public override async Task DisposeAsync()
        {
            await using (var session = Neo4j.Driver.AsyncSession())
                await session.WriteTransactionAsync(tx =>
                        tx.RunAsync("MATCH (u:User {email:$email}) DETACH DELETE u",
                    new {email = Email}));

            await base.DisposeAsync();
        }

        [Fact]
        public async Task Should_find_a_unique_constraint()
        {
            await using var session = Neo4j.Driver.AsyncSession();

            var result = await session.ReadTransactionAsync(async tx =>
                {
                    var cursor = await tx.RunAsync(@"
                        CALL db.constraints()
                        YIELD name, description
                        WHERE description = 'CONSTRAINT ON ( user:User ) ASSERT (user.email) IS UNIQUE'
                        RETURN *");
                    return await cursor.ToListAsync();
                });

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task Should_throw_a_ValidationException_when_email_already_exists()
        {
            var service = new AuthService(Neo4j.Driver);

            var output = await service.RegisterAsync(Email, Password, Name);

            Assert.Equal(Email, output["email"]);
            Assert.Equal(Name, output["name"]);
            Assert.Throws<KeyNotFoundException>(() => output["password"]);
            Assert.False(string.IsNullOrEmpty(output["token"].ToString()));

            await Assert.ThrowsAsync<ValidationException>(async () =>
                await service.RegisterAsync(Email, Password, Name));
        }
    }
}
