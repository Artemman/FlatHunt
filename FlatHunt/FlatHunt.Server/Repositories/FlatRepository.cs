using FlatHunt.Server.Data;
using FlatHunt.Server.Dto;
using FlatHunt.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlatHunt.Server.Repositories
{
    public class FlatRepository : IFlatRepository
    {
        private readonly AppDbContext _db;

        public FlatRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(List<FlatDto> Items, long TotalCount)> GetFilteredAsync(FlatFilterRequest filter, CancellationToken ct = default)
        {
            var page = Math.Max(1, filter.Page);
            var pageSize = Math.Clamp(filter.PageSize, 1, 100);

            var baseQuery = _db.Flats
                .Select(f => new
                {
                    Flat = f,
                    Ad = f.Advertisements
                        .OrderByDescending(a => a.CreatedAt)
                        .FirstOrDefault()
                })
                .AsQueryable();

            // Filters on flat fields
            if (filter.Rooms.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.Flat.RoomCount == filter.Rooms.Value);
            }

            if (filter.MinArea.HasValue)
            {
                baseQuery = baseQuery.Where(x => (x.Flat.AreaTotal ?? 0m) >= filter.MinArea.Value);
            }

            if (filter.MaxArea.HasValue)
            {
                baseQuery = baseQuery.Where(x => (x.Flat.AreaTotal ?? 0m) <= filter.MaxArea.Value);
            }

            // Filters on advertisement (price/search)
            if (filter.MinPrice.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.Ad != null && x.Ad.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                baseQuery = baseQuery.Where(x => x.Ad != null && x.Ad.Price <= filter.MaxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var s = filter.Search.ToLower();
                baseQuery = baseQuery.Where(x =>
                    (x.Ad != null && (
                        (x.Ad.Title ?? "").ToLower().Contains(s) ||
                        (x.Ad.Address ?? "").ToLower().Contains(s)
                    ))
                );
            }

            if (filter.CityId.HasValue)
            {
                //baseQuery = baseQuery.Where(x =>
                //    x.Ad != null && x.Ad.CityId == filter.CityId.Value);
            }

            // Total count before pagination
            var totalCount = await baseQuery.LongCountAsync(ct).ConfigureAwait(false);

            // Sorting
            var sortBy = (filter.SortBy ?? "").ToLowerInvariant();
            var desc = (filter.SortDir ?? "desc").ToLowerInvariant() != "asc";

            baseQuery = sortBy switch
            {
                "price" => desc
                    ? baseQuery.OrderByDescending(x => x.Ad != null ? x.Ad.Price : (decimal?)null)
                    : baseQuery.OrderBy(x => x.Ad != null ? x.Ad.Price : (decimal?)null),
                "area" => desc
                    ? baseQuery.OrderByDescending(x => x.Flat.AreaTotal)
                    : baseQuery.OrderBy(x => x.Flat.AreaTotal),
                "createdat" => desc
                    ? baseQuery.OrderByDescending(x => x.Ad != null ? x.Ad.CreatedAt : (DateTime?)null)
                    : baseQuery.OrderBy(x => x.Ad != null ? x.Ad.CreatedAt : (DateTime?)null),
                _ => baseQuery.OrderByDescending(x => x.Ad != null ? x.Ad.CreatedAt : (DateTime?)null)
            };

            var paged = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new FlatDto
                {
                    Id = x.Flat.Id,
                    Title = x.Ad != null ? x.Ad.Title : null,
                    Address = x.Ad != null ? x.Ad.Address : null,
                    CityId = null, // adapt if City relation exists
                    Rooms = x.Flat.RoomCount,
                    Area = x.Flat.AreaTotal,
                    Price = x.Ad != null ? x.Ad.Price : (decimal?)null,
                    Currency = x.Ad != null ? x.Ad.Currency : null,
                    IsActive = true,
                    CreatedAt = x.Ad != null ? x.Ad.CreatedAt : null
                })
                .ToListAsync(ct)
                .ConfigureAwait(false);

            return (paged, totalCount);
        }
    }
}