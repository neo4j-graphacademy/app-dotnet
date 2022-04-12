using System;
using System.Threading.Tasks;
using Neo4j.Driver;

namespace Neoflix
{
    public static class Neo4j
    {
        // tag::driver[]
        private static IDriver _driver = null;
        // end::driver[]
        
        // tag::getDriver[]
        public static IDriver Driver => _driver;
        // end::getDriver[]

        // tag::initDriver[]
        public static async Task InitDriverAsync(string uri, string username, string password)
        {
            //throw new NotImplementedException("Create an instance of the driver here");

        }
        // end::initDriver[]

        // tag::closeDriver[]
        public static Task CloseDriver()
        {
            return Driver != null ? Driver.CloseAsync() : Task.CompletedTask;
        }
        // end::closeDriver[]
    }
}