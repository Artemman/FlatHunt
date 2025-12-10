using FlatHunt.Server.Models;
using FlatHunt.Server.Services.Parser.Interfaces;
using FlatHunt.Server.Services.Sync.Interfaces;
using FlatHunt.Server.Services.Sync.Models;

namespace FlatHunt.Server.Services.Sync
{
    public class FlatSyncService : IFlatSyncService
    {
        private readonly ILogger<FlatSyncService> _logger;
        private readonly ILunFlatParserService _lunAdvertisementParserService;

        public FlatSyncService(ILogger<FlatSyncService> logger, ILunFlatParserService lunAdvertisementParserService)
        {
            _logger = logger;
            _lunAdvertisementParserService = lunAdvertisementParserService;

        }

        public async Task<List<Advertisement>> SyncFlats(FlatSyncFilter filter)
        {
            var syncId = Guid.NewGuid();
            _logger.LogInformation("Start sync advertisements session id: {0}", syncId);

            var parserService = GetParser(filter.FlatSourceType);
            var advertisements = await parserService.ParseFlats(filter);

            _logger.LogInformation("End sync advertisements session id: {0}, count: {1}", syncId, advertisements.Count);

            return advertisements;
        }

        private IFlatParserService GetParser(FlatSourceType source)
        {
            return source switch
            {
                FlatSourceType.Lun => _lunAdvertisementParserService,
                FlatSourceType.FlatFy => throw new NotImplementedException("Flat fy sync not implemented"),
                _ => throw new ArgumentException("Unknown source type", nameof(source)),
            };
        }
    }
}
