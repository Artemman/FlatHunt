using FlatHunt.Server.Models;
using FlatHunt.Server.Repositories.Interfaces;
using FlatHunt.Server.Services.FlatProviders.Lun.Dto;
using FlatHunt.Server.Services.FlatProviders.Lun.Interfaces;
using FlatHunt.Server.Services.Parser.Interfaces;
using FlatHunt.Server.Services.Parser.Models;
using System.Text.Json;

namespace FlatHunt.Server.Services.Parser
{
    public class LunFlatParserService : ILunFlatParserService
    {
        private const string Language = "uk";
        private const int SectionId = 2; //TODO find what is that?

        private readonly ILogger<LunFlatParserService> _logger;
        private readonly ILunClient _lunClient;
        private readonly ICityRepository _cityRepository;
        private readonly IAdvertisementRepository _advertisementRepository;

        public LunFlatParserService(
            ILogger<LunFlatParserService> logger,
            ILunClient lunClient,
            ICityRepository cityRepository,
            IAdvertisementRepository advertisementRepository)
        {
            _logger = logger;
            _lunClient = lunClient;
            _cityRepository = cityRepository;
            _advertisementRepository = advertisementRepository;
        }

        public async Task<List<Advertisement>> ParseFlats(FlatParseFilter filter)
        {
            string geoId = await GetLunGeoId(filter.CityId);


            var response = await _lunClient.GetRealtiesAsync(Language, SectionId,
                filter.RoomCount, filter.Page, geoId, filter.PageSize);

            if (!response.IsSuccessStatusCode
                || response.Content == null)
            {
                //TODO use custom exception type 
                //remove IsSuccessStatusCode it after handler will be added 
                throw new Exception("Lun API unavailable");
            }

            //todo add serilog/nlog sinks and logging to file on debug

#if DEBUG
            _logger.LogInformation(JsonSerializer.Serialize(response.Content));
#endif
            if (response.Content.Cards == null
                || response.Content.Cards.Count == 0) 
            {
                return [];
            }

            var advertisements = new List<Advertisement>();
            response.Content.Cards.ForEach(card => Parse(card, advertisements));

            await _advertisementRepository.SaveChanges();

            return advertisements;
        }

        private async Task<string> GetLunGeoId(int cityId)
        {
            var city = await _cityRepository.GetById(cityId, x => x.LunCity!);
            ArgumentException.ThrowIfNullOrWhiteSpace(city?.LunCity?.GeoId);

            return city.LunCity.GeoId;
        }

        private void Parse(LunCardDto card, List<Advertisement> advertisements)
        {
            //todo add deduplication service 
            var advertisement = new Advertisement()
            {
                FlatSourceType = FlatSourceType.Lun,
                ExternalId = card.Id.ToString(),
                Url = card.UrlRaw!,
                Title = card.Header!,
                Description = card.Text!,
                Price = card.Price,
                Flat = new()
                {
                    RoomCount = card.RoomCount,
                    Floor = card.Floor,
                    AreaTotal = card.AreaTotal,
                    FloorCount = card.FloorCount
                }
            };

            advertisements.Add(advertisement);
            _advertisementRepository.Add(advertisement);
            
        }
    }
}
