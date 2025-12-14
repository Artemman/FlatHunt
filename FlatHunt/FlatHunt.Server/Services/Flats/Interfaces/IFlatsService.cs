using FlatHunt.Server.Dto;

namespace FlatHunt.Server.Services.Flats.Interfaces
{
    public interface IFlatsService
    {
        Task<PagedResult<FlatDto>> GetFlatsAsync(FlatFilterRequest filter, CancellationToken ct = default);
    }
}