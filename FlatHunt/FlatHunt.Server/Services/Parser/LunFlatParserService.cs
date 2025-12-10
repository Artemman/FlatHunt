using FlatHunt.Server.Models;
using FlatHunt.Server.Services.FlatProviders.Lun.Interfaces;
using FlatHunt.Server.Services.Parser.Interfaces;
using FlatHunt.Server.Services.Parser.Models;

namespace FlatHunt.Server.Services.Parser
{
    public class LunFlatParserService : ILunFlatParserService
    {
        private const string Language = "uk";
        private const int SectionId = 2; //TODO find what is that?

        private ILunClient _lunClient;
        private ICityRepository _cityRepository;

        public LunFlatParserService(ILunClient lunClient)
        {
            _lunClient = lunClient;
        }

        public async Task<List<Advertisement>> ParseFlats(FlatParseFilter filter)
        {
            var geoId = 
            var response = await _lunClient.GetRealtiesAsync(Language, SectionId,
                filter.RoomCount, filter.Page)
        }
    }
}
