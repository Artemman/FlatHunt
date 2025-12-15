using FlatHunt.Server.Dto;

namespace FlatHunt.Server.Repositories.Interfaces
{
    public interface IFlatRepository
    {
        Task<(List<FlatDto> Items, long TotalCount)> GetFilteredAsync(FlatFilterRequest filter);
    }
}