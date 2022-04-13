using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Neoflix
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // load config from appsettings.json
            var (uri, user, password) = Config.UnpackNeo4jConfig();

            // connect to Neo4J and Verify Connectivity
            await Neo4j.InitDriverAsync(uri, user, password);

            // configure and run website
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}