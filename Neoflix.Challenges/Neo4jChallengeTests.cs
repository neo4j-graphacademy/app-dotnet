using System.Threading.Tasks;
using Xunit;

namespace Neoflix.Challenges
{
    public abstract class Neo4jChallengeTests : IAsyncLifetime
    {
        public virtual async Task InitializeAsync()
        {
            var (uri, user, password) = Config.UnpackNeo4jConfig();
            await Neo4j.InitDriverAsync(uri, user, password);
        }

        public virtual async Task DisposeAsync()
        {
            await Neo4j.CloseDriver();
        }
    }
}