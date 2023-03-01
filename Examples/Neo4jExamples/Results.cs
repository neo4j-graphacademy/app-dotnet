using Neo4j.Driver;

//this code is not legal and is for use in ascii docs only.
public class Results
{
    async Task Main(IDriver driver)
    {
        using var session = driver.AsyncSession();

        var res = await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync(@"
                    MATCH path = (person:Person)-[actedIn:ACTED_IN]->(movie:Movie)
                    RETURN path, person, actedIn, movie,
                           size((person)-[:ACTED]->()) as movieCount,
                           exists { (person)-[:DIRECTED]->() } as isDirector
                    LIMIT 1
                ");

            // tag::records[]
            // Get single row, error when more/less than 1
            IRecord row = await cursor.SingleAsync();
            // Materialize list
            List<IRecord> rows = await cursor.ToListAsync();

            // iteration
            while (await cursor.FetchAsync())
            {
                IRecord next = cursor.Current;
            }
            // end::records[]

            // tag::record[]
            // column names
            IReadOnlyList<string> columns = row.Keys;
            // check for existence
            bool movieExists = row.Values.ContainsKey("movie");
            // number of columns
            int columnCount = row.Values.Count;
            // get a numeric value (int, long, float, double)
            int count = row["movieCount"].As<int>(0);
            // get a bool value
            bool isDirector = row["isDirector"].As<bool>();
            // get node
            INode node = row["movie"].As<INode>();
            // end::record[]

            // tag::get[]
            // Get a node
            INode person = row["person"].As<INode>();
            // end::get[]

            // Working with Nodes
            // tag::node[]
            long nodeId = person.Id; // (1)
            IReadOnlyList<string> labels = person.Labels; // (2)
            IReadOnlyDictionary<string, object> properties = person.Properties; // (3)
            // end::node[]

            // Working with Relationships
            // tag::rel[]
            IRelationship actedIn = row["actedIn"].As<IRelationship>();

            long relId = actedIn.Id; // (1)
            string type = actedIn.Type; // (2)
            IReadOnlyDictionary<string, object> relProperties = actedIn.Properties;
            long startId = actedIn.StartNodeId;
            long endId = actedIn.EndNodeId;
            // end::rel[]


            // Working with Paths
            // tag::path[]
            IPath path = row["path"].As<IPath>();

            INode start = path.Start; // (1)
            INode end = path.End; // (2)

            IEnumerable<INode> nodes = path.Nodes; // (3)
            IEnumerable<IRelationship> rels = path.Relationships; // (4)
            // end::path[]

            // tag::summary[]
            IResultSummary summary = await cursor.ConsumeAsync();
            // Time in milliseconds before receiving the first result
            TimeSpan availableAfter = summary.ResultAvailableAfter; // 10
            // Time in milliseconds once the final result was consumed
            TimeSpan consumedAfter = summary.ResultConsumedAfter; // 10
                                                                  // end::summary[]

            // tag::summary:counters[]
            ICounters counters = summary.Counters;
            // some example counters
            // nodes and relationships
            bool containsUpdates = counters.ContainsUpdates;
            int nodesCreated = counters.NodesCreated;
            int labelsAdded = counters.LabelsAdded;
            int relsDeleted = counters.RelationshipsDeleted;
            int propertiesSet = counters.PropertiesSet;

            // indexes and constraints
            int indexesAdded = counters.IndexesAdded;
            int constraintsRemoved = counters.ConstraintsRemoved;
            // updates to system db
            bool containsSystemUpdates = counters.ContainsSystemUpdates;
            int systemUpdates = counters.SystemUpdates;
            // end::summary:counters[]

            // tag::summary:infra[]
            Query query = summary.Query;
            QueryType queryType = summary.QueryType;
            IList<INotification> notifications = summary.Notifications;
            IDatabaseInfo database = summary.Database;
            IServerInfo server = summary.Server;
            // end::summary:infra[]

            // tag::summary:plan[]
            bool hasPlan = summary.HasPlan;
            IPlan plan = summary.Plan;
            bool hasProfile = summary.HasProfile;
            IProfiledPlan profile = summary.Profile;
            // end::summary:plan[]

            // tag::temporal[]
            var released = node["released"];
            var localDate = released.As<LocalDate>();
            var localDateTime = released.As<LocalDateTime>();
            var localTime = released.As<LocalTime>();
            var dateTimeOffset = released.As<DateTimeOffset>();
            // end::temporal[]

            // tag::spatial[]
            Point loc = node["location"].As<Point>();
            // end::spatial[]

            return 1;
        });
    }
}
