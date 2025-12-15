using FlatHunt.Server.Dto;
using FlatHunt.Server.Repositories.Interfaces;
using FlatHunt.Server.Services.Flats.Interfaces;

namespace FlatHunt.Server.Services.Flats
{
    public class FlatsService : IFlatsService
    {
        private readonly IFlatRepository _repo;

        public FlatsService(IFlatRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<FlatDto>> GetFlatsAsync(FlatFilterRequest filter, CancellationToken ct = default)
        {
            var page = Math.Max(1, filter.Page);
            var pageSize = Math.Clamp(filter.PageSize, 1, 100);

            var (items, total) = await _repo.GetFilteredAsync(filter).ConfigureAwait(false);

            var totalPages = (int)Math.Ceiling(total / (double)pageSize);

            return new PagedResult<FlatDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                TotalPages = totalPages
            };
        }
    }
}