using Neo4j.Driver;

public class ValidationException : Exception
{
    public object Details;

    public ValidationException(string message, object details) : base(message)
    {
        Details = details;
    }
}

internal class Exceptions
{
    private string username = "neo4j";
    private string password = "letmein!";

    async Task Main(IDriver driver)
    {
        var email = "uniqueconstraint@example.com";

        // tag::constraint-error[]
        using var session = driver.AsyncSession();
        try
        {
            await session.ExecuteWriteAsync(async tx =>
            {
                var cursor = await tx.RunAsync(
                    "CREATE (u: User { email: $email }) RETURN u",
                    new {email});
                var result = await cursor.SingleAsync();
                return result["u"].As<INode>().Properties;
            });
        }
        catch (Neo4jException ex) when (ex.Code == "Neo.ClientError.Schema.ConstraintValidationFailed")
        {
            throw new ValidationException($"An account already exists with the email address.", new { email });
        }
        // end::constraint-error[]
    }
}
