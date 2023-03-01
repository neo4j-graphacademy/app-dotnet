// tag::import[]
// Import all relevant classes from neo4j-dotnet-driver
using Neo4j.Driver;
// end::import[]

// tag::credentials[]
var username = "neo4j";
var password = "letmein!";
// end::credentials[]

// tag::auth[]
var authenticationToken = AuthTokens.Basic(username, password);
// end::auth[]

/*
 * Here is the pseudocode for creating the Driver:
 *
// tag::pseudo[]
var driver = GraphDatabase.Driver(
  connectionString, // <1>
  authenticationToken, // <2>
  configuration // <3>
)
// end::pseudo[]

 * The first argument is the connection string, it is constructed like so:
// tag::connection[]
  address of server
          ↓
neo4j://localhost:7687
  ↑                ↑
scheme         port number
// end::connection[]
 *
 */

// The following code creates an instance of the Neo4j Driver
// tag::driver[]
var driver = GraphDatabase.Driver("bolt://localhost:7687",
    AuthTokens.Basic(username, password));
// end::driver[]

// tag::configuration[]
var configBuilder = (ConfigBuilder builder) =>
{
    builder
        .WithConnectionTimeout(TimeSpan.FromSeconds(30))
        .WithMaxConnectionLifetime(TimeSpan.FromMinutes(30))
        .WithMaxConnectionPoolSize(10)
        .WithConnectionAcquisitionTimeout(TimeSpan.FromSeconds(20))
        .WithFetchSize(1000);
};
// end::configuration[]

/*
 * It is considered best practice to inject an instance of the driver.
 * This way the object can be mocked within unit tests
 */
public class ExampleClass
{
    private readonly IDriver _driver;

    public ExampleClass(IDriver driver)
    {
        _driver = driver;
    }

    public void Method()
    {
        // tag::session[]
        // Open a new session
        using var session = _driver.AsyncSession();
        // Do something with the session...

        // end::session[]
        // Close the session automatically when session is disposed.
    }

/*
 * These functions are wrapped in an `async` function so that we can use the await
 * keyword rather than the Promise API.
 */
    public async Task Main(IDriver driver)
    {
        // tag::verifyConnectivity[]
        // Verify the connection details
        await driver.VerifyConnectivityAsync();
        // end::verifyConnectivity[]

        Console.WriteLine("Connection Verified!");

        // tag::driver.session[]
        // Open a new session
        var session = driver.AsyncSession();
        // end::driver.session[]

        // tag::session.run[]
        var query = "MATCH () RETURN count(*) AS count";
        var queryParams = new Dictionary<string, object>();

        // Run a query in an auto-commit transaction
        var cursor = await session.RunAsync(query, queryParams);
        var record = await cursor.SingleAsync();
        var result = record["count"].As<long>();
        // end::session.run[]

        Console.WriteLine(result);


        // tag::session.close[]
        // Close the session
        await session.CloseAsync();
        // end::session.close[]
    }

    private async Task ShowReadTransaction(IDriver driver)
    {
        using var session = driver.AsyncSession();

        // tag::session.readTransaction[]
        // Run a query within a Read Transaction
        var res = await session.ExecuteReadAsync(async tx => {
            var cursor = await tx.RunAsync(@"
                MATCH(p: Person) -[:ACTED_IN]->(m: Movie)
                WHERE m.title = $title // <1>
                RETURN p.name AS name
                LIMIT 10
            ", new { title = "Arthur" });

            var records = await cursor.ToListAsync();
            return records.Select(x => x["name"].As<string>());
        });
        // end::session.readTransaction[]
    }

    private async Task ShowWriteTransaction(IDriver driver)
    {
        using var session = driver.AsyncSession();
        // tag::session.writeTransaction[]
        await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync(
                "CREATE (p:Person {name: $name})",
                new {name = "Michael"});
            await cursor.ConsumeAsync();
        });
        // end::session.writeTransaction[]
    }

    private async Task ShowManualTransaction(IDriver driver)
    {
        // tag::session.beginTransaction[]
        // Open a new session
        using var session = driver.AsyncSession(configBuilder =>
        {
            configBuilder.WithDefaultAccessMode(AccessMode.Write);
        });
        // Manually create a transaction
        var tx = await session.BeginTransactionAsync();
        // end::session.beginTransaction[]

        var query = "MATCH (n) RETURN count(n) AS count";

        // tag::session.beginTransaction.Try[]
        try
        {
            // Perform an action
            await tx.RunAsync(query);

            // Commit the transaction
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            // If something went wrong, rollback the transaction
            await tx.RollbackAsync();
        }
        // end::session.beginTransaction.Try[]
    }

    /// <summary>
    /// This is an example function that will create a new Person node
    /// within a write transaction and return the properties for the node.
    ///
    /// </summary>
    /// <param name="driver">The driver.</param>
    /// <param name="name">The name of a new person.</param>
    /// <returns>Task representing async operation</returns>
    // tag::createPerson[]
    private async Task<Dictionary<string, object>> CreatePerson(IDriver driver, string name)
    {
        // tag::sessionWithArgs[]
        // Create a Session for the `people` database
        using var session = driver.AsyncSession(configBuilder =>
            configBuilder
                .WithDefaultAccessMode(AccessMode.Write)
                .WithDatabase("people"));
        // end::sessionWithArgs[]

        // Create a node within a write transaction
        var result = await session.ExecuteWriteAsync(async tx =>
        {
            var cursor = await tx.RunAsync("CREATE (p:Person {name: $name}) RETURN p",
                new {name});
            return await cursor.SingleAsync();
        });

        // Get the `p` value from the first record
        var node = result["p"].As<INode>();

        // Return the properties of the node
        return node.Properties
            .ToDictionary(x => x.Key, x => x.Value);
    }
    // end::createPerson[]
}