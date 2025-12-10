using Refit;
using FlatHunt.Server.Services.FlatProviders.Lun.Dto;

namespace FlatHunt.Server.Services.FlatProviders.Lun.Interfaces
{
    public interface ILunClient
    {
        //add handler and remove IApiResponse
        [Get("/api/v2/market/realties")]
        Task<IApiResponse<LunResponseDto>> GetRealtiesAsync(
        [AliasAs("language")] string language,
        [AliasAs("sectionId")] int sectionId,
        [AliasAs("roomCount")] int roomCount,
        [AliasAs("page")] int page,
        [AliasAs("geoId")] string geoId,
        [AliasAs("pageSize")] int pageSize,
        [AliasAs("geoDistance")] string? geoDistance = null);
    }

    
}
