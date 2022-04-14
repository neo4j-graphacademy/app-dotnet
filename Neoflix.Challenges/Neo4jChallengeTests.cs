using System.Threading.Tasks;
using NUnit.Framework;

namespace Neoflix.Challenges
{
    [TestFixture]
    [NonParallelizable]
    public abstract class Neo4jChallengeTests
    {
        [OneTimeSetUp]
        public virtual async Task SetupAsync()
        {
            var (uri, user, password) = Config.UnpackNeo4jConfig();
            await Neo4j.InitDriverAsync(uri, user, password);
        }

        [OneTimeTearDown]
        public virtual async Task TeardownAsync()
        {
            await Neo4j.CloseDriver();
        }
    }
}