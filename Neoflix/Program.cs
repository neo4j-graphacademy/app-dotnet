using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Neoflix
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var (uri, user, password) = Config.UnpackNeo4jConfig();
            await Neo4j.InitDriverAsync(uri, user, password);

            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}