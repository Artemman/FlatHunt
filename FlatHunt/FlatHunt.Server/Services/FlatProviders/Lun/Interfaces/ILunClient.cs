using Refit;
using FlatHunt.Server.Services.FlatProviders.Lun.Dto;

namespace FlatHunt.Server.Services.FlatProviders.Lun.Interfaces
{
    public interface ILunClient
    {
        [Get("/api/v2/market/realties")]
        Task<LunResponseDto> GetRealtiesAsync(
        [AliasAs("language")] string language,
        [AliasAs("sectionId")] int sectionId,
        [AliasAs("roomCount")] int roomCount,
        [AliasAs("page")] int page,
        [AliasAs("geoId")] long geoId,
        [AliasAs("pageSize")] int pageSize,
        [AliasAs("groupCollapse")] bool groupCollapse,
        [AliasAs("geoDistance")] string geoDistance);
    }

    
}
