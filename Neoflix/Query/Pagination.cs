namespace Neoflix
{
    public class Pagination
    {
        public string Query { get; init; }
        public string Sort { get; init; }
        public Ordering Order { get; init; }
        public int Limit { get; init; }
        public int Skip { get; init; }
    }
}