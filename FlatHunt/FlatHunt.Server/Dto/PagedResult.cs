using System.Collections.Generic;

namespace FlatHunt.Server.Dto
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();

        public int Page { get; init; }

        public int PageSize { get; init; }

        public long TotalCount { get; init; }

        public int TotalPages { get; init; }
    }
}