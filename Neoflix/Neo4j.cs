using System.Threading.Tasks;
using Neo4j.Driver;

namespace Neoflix
{
    /// <summary>
    /// Static class to maintain the singleton instance of the driver.
    /// </summary>
    public static class Neo4j
    {
        /// <summary>
        /// A singleton instance of the Neo4j Driver to be used across the app.
        /// </summary>
        // tag::driver[]
        private static IDriver _driver = null;
        // end::driver[]
        
        // tag::getDriver[]
        public static IDriver Driver => _driver;
        // end::getDriver[]

        /// <summary>
        /// Initializes the Neo4j Driver.
        /// </summary>
        /// <param name="uri">Neo4j server URI, e.g. "neo4j://localhost:7687".</param>
        /// <param name="username">Username to connect to Neo4j with, e.g. "neo4j".</param>
        /// <param name="password">Password for the user.</param>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
        // tag::initDriver[]
        public static async Task InitDriverAsync(string uri, string username, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));
            await _driver.VerifyConnectivityAsync();
        }
        // end::initDriver[]

        /// <summary>
        /// If the driver has been instantiated, close it and all remaining open sessions.
        /// </summary>
        /// <returns>A task that represents the asynchronous close operation.</returns>
        // tag::closeDriver[]
        public static Task CloseDriver()
        {
            return _driver != null ? _driver.CloseAsync() : Task.CompletedTask;
        }
        // end::closeDriver[]
    }
}