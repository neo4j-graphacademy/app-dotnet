using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Neoflix
{
    public class HttpRequestUtils
    {
        private static readonly Dictionary<SortType, string[]> _validSorts = new()
        {
            [SortType.Movies] = new[] {"title", "released", "imdbRating"},
            [SortType.People] = new[] {"name", "born", "movieCount"},
            [SortType.Rating] = new[] {"rating", "timestamp"}
        };

        public static (string query, string sort, Ordering order, int limit, int skip)
            GetPagination(IQueryCollection queryCollection, SortType? sortType = null)
        {
            var query = default(string);
            var sort = default(string);
            var order = Ordering.Asc;
            var limit = 6;
            var skip = 0;

            if (queryCollection.TryGetValue("q", out var qsQuery))
                query = qsQuery.ToString();

            if (queryCollection.TryGetValue("sort", out var qsSort) && sortType.HasValue)
                if (_validSorts[sortType.Value].Contains(qsSort.ToString()))
                    sort = qsSort.ToString();

            if (queryCollection.TryGetValue("order", out var qsOrder))
                order = qsOrder.ToString() switch
                {
                    "ASC" => Ordering.Asc,
                    "DESC" => Ordering.Desc,
                    _ => order
                };

            if (queryCollection.TryGetValue("limit", out var qsLimit))
                int.TryParse(qsLimit, out limit);

            if (queryCollection.TryGetValue("skip", out var qsSkip))
                int.TryParse(qsSkip, out skip);

            return (query, sort, order, limit, skip);
        }

        public static string GetUserId(HttpRequest request)
        {
            if (request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var encoded = authHeader.ToString().Replace("Bearer ", string.Empty);

                var encodedParts = encoded.Split(".");
                var payload = JwtPayload.Base64UrlDeserialize(encodedParts[1]);

                return payload.Sub;
            }

            return null;
        }
    }
}